import React, { useState, useEffect, useMemo } from 'react';
import { useForm, FormProvider } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { IApplicationMain } from 'types';
import { yupResolver } from '@hookform/resolvers';
import { useMutation, useQueryCache } from 'react-query';
import {
    useApplication,
    useApplicationMain,
    useRefinancingLoan,
    useLoanParameters,
    useScoringResult,
    useLoanTypes,
    useLoanLimits
} from 'hooks';
import { saveApplicationMain, saveRefinancingLoan } from 'queryies';
import { getAddressesFromProfile, getRepayDayValue } from 'helpers/data';
import LoadingButton from 'components/LoadingButton';
import { FieldSet } from 'components/fieldset';
import { useUserState } from 'providers/UserProvider';
import { Alert } from 'react-bootstrap';
import { applicationDetailsScheme, validateRefinancing } from '../validations';
import ApplicationDetailsForm from '../components/ApplicationDetailsForm';
import { useApplicationDispatch } from '../Provider';

type Props = {
    id: string;
    disabled: boolean;
};

function ApplicationDetails({ id, disabled }: Props) {
    const navigate = useNavigate();
    const [saveLoading, setSaveLoading] = useState(false);
    const [submitError, setSubmitError] = useState(false);
    const queryCache = useQueryCache();
    const profileData = useUserState();
    const { setActiveTabName } = useApplicationDispatch();

    const [validatationContext, setValidationContext] = useState<any>({
        clientCode: false,
        loanParameters: null,
        isRefinancing: false
    });

    const methods = useForm<IApplicationMain>({
        resolver: yupResolver(applicationDetailsScheme),
        context: validatationContext
    });
    const { handleSubmit, getValues, reset, watch } = methods;

    const interestId = watch('INTEREST');

    const isStudent = profileData?.IS_STUDENT;

    // useQueryies
    const { data: loanTypes } = useLoanTypes(isStudent, { enabled: isStudent !== undefined });

    const { data: scoringResult } = useScoringResult(id, {
        enabled: !!id
    });

    const interest = useMemo(() => {
        return scoringResult?.find(({ INTEREST }) => INTEREST === Number(interestId));
    }, [scoringResult, interestId]);

    const { data: refinancingData } = useRefinancingLoan(id, {
        enabled: !!id
    });

    const { data: applicationData, refetch } = useApplication(id, {
        enabled: false
    });
    const clientCode = applicationData?.CLIENT_CODE?.trim();
    const loanTypeId = applicationData?.LOAN_TYPE_ID;
    const isRefinancing = applicationData?.IS_REFINANCING;
    const currencyCode = applicationData?.CURRENCY_CODE;

    const { data: loanLimits } = useLoanLimits(loanTypeId, currencyCode);

    const fromAmount = loanLimits?.FROM_AMOUNT;

    const { data: applicationMainData } = useApplicationMain(id, {
        enabled: !!id
    });
    const loanType = useMemo(() => {
        return loanTypes?.find(({ CODE }) => CODE === loanTypeId);
    }, [loanTypes, loanTypeId]);

    const { data: loanParameters } = useLoanParameters(loanTypeId, {
        enabled: !!loanTypeId
    });
    const isRepayStartDay = loanParameters?.IS_REPAY_START_DAY ?? false;

    const disableRepayDay = useMemo(() => loanParameters?.IS_REPAY_DAY_FIXED, [loanParameters]);

    const getDataForSubmit = (values: IApplicationMain) => {
        const { LOAN_CODES, DOC_PASSPORT, DOC_SOC_CARD, ...rest } = values;
        rest.ID = id;
        rest.MOBILE_PHONE_1 = rest.MOBILE_PHONE;
        rest.FINAL_AMOUNT = rest.AMOUNT;
        rest.LOAN_TYPE_STATE = loanType ? loanType.STATE : null;
        rest.AGREED_WITH_TERMS = true;
        if (loanType?.IS_OVERDRAFT) {
            rest.OVERDRAFT_TEMPLATE_CODE = interest ? interest.TEMPLATE_CODE : null;
        } else {
            rest.LOAN_TEMPLATE_CODE = interest ? interest.TEMPLATE_CODE : null;
        }
        return rest;
    };

    // mutations
    const [mutate, { isLoading: isMainAppSubmitting }] = useMutation(saveApplicationMain, {
        onSuccess: async () => {
            queryCache.invalidateQueries(['APPLICATION', id]);
            const data = await refetch();
            if (data?.STATUS_ID === 11) {
                setActiveTabName('VERIFICATION');
            } else {
                navigate('/');
            }
        }
    });

    const onSubmit = (values: IApplicationMain) => {
        const data = getDataForSubmit(values);

        if (isRefinancing) {
            const { LOAN_CODES } = values;

            if (validateRefinancing(refinancingData, LOAN_CODES, fromAmount, interest?.AMOUNT)) {
                setSubmitError(false);
                mutate({ ...data, SUBMIT: true });
                saveRefinancingLoan(values.LOAN_CODES);
            } else {
                setSubmitError(true);
            }
        } else {
            mutate({ ...data, SUBMIT: true });
        }
    };

    const handleSaveForm = async () => {
        setSaveLoading(true);
        const formData = getValues();
        const data = getDataForSubmit(formData);

        try {
            await saveApplicationMain({
                ...data,
                SUBMIT: false
            });

            if (isRefinancing) {
                await saveRefinancingLoan(formData.LOAN_CODES);
            }
        } finally {
            setSaveLoading(false);
        }
    };

    useEffect(() => {
        if (clientCode) {
            setValidationContext((prev: any) => ({ ...prev, clientCode: true }));
        }
    }, [clientCode]);

    useEffect(() => {
        if (profileData) {
            const addresses = getAddressesFromProfile(profileData);
            const optionalData: { REPAY_DAY?: string } = {};

            if (disableRepayDay && loanTypeId) {
                optionalData.REPAY_DAY = getRepayDayValue(loanTypes, loanTypeId, isRepayStartDay);
            }

            reset({
                ...addresses,
                ...{
                    FIRST_NAME_EN: profileData.FIRST_NAME_EN,
                    LAST_NAME_EN: profileData.LAST_NAME_EN,
                    BIRTH_PLACE_CODE: profileData.BIRTH_PLACE_CODE,
                    CITIZENSHIP_CODE: profileData.CITIZENSHIP_CODE,
                    MOBILE_PHONE: profileData.MOBILE_PHONE,
                    FIXED_PHONE: profileData.FIXED_PHONE,
                    EMAIL: profileData.EMAIL,
                    LOAN_CODES: refinancingData
                },
                ...applicationMainData,
                ...optionalData
            });
        }
    }, [
        reset,
        applicationMainData,
        profileData,
        refinancingData,
        loanTypes,
        loanTypeId,
        disableRepayDay,
        isRepayStartDay
    ]);

    useEffect(() => {
        if (loanParameters) {
            setValidationContext((prev: any) => ({ ...prev, loanParameters }));
        }
    }, [loanParameters]);

    useEffect(() => {
        if (isRefinancing) {
            setValidationContext((prev: any) => ({ ...prev, isRefinancing }));
        }
    }, [isRefinancing]);

    return (
        <FieldSet className="application-form-wrapper" disabled={disabled}>
            <FormProvider {...methods}>
                <ApplicationDetailsForm
                    onSubmit={handleSubmit(onSubmit)}
                    id={id}
                    scoringResult={scoringResult}
                    interest={interest}
                    isTabDisabled={disabled}
                    disableRepayDay={disableRepayDay}
                />
                {submitError && (
                    <Alert variant="danger">
                        <h4>Ուշադրություն`</h4>
                        Վերաֆինանսավորման համար անհրաժեշտ գումարը պետք է լինի {fromAmount}դր․ -ից{' '}
                        {interest?.AMOUNT}դր․ միջակայքում
                    </Alert>
                )}

                <div className="form-actions">
                    <LoadingButton
                        variant="outline-primary"
                        loading={saveLoading}
                        onClick={handleSaveForm}
                    >
                        Հիշել
                    </LoadingButton>

                    <LoadingButton
                        type="submit"
                        variant="primary"
                        form="ApplicationDetailsForm"
                        loading={isMainAppSubmitting}
                    >
                        Շարունակել
                    </LoadingButton>
                </div>
            </FormProvider>
        </FieldSet>
    );
}

export default ApplicationDetails;
