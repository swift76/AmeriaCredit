import React, { useState, useCallback, useEffect } from 'react';
import { IApplication } from 'types';
import { yupResolver } from '@hookform/resolvers';
import differenceInMinutes from 'date-fns/differenceInMinutes';
import { useNavigate, Link, useParams } from 'react-router-dom';
import { useForm, FormProvider } from 'react-hook-form';
import { useMutation, useQueryCache } from 'react-query';
import { Modal, Button } from 'react-bootstrap';
import { getIdentityFromProfile } from 'helpers/data';
import { useApplication, useLoanLimits } from 'hooks';
import { FieldSet } from 'components/fieldset';
import { saveApplication } from 'queryies';
import LoadingButton from 'components/LoadingButton';
import { useUserState } from 'providers/UserProvider';
import RequestModalContent from '../components/RequestModalContent';
import ApplicationBaseForm from '../components/ApplicationBaseForm';
import { applicationBaseScheme } from '../validations';
import { useApplicationDispatch, useApplicationState } from '../Provider';

type Props = {
    disabled: boolean;
};

function ApplicationBase({ disabled }: Props) {
    const { setActiveTabName, setModalState } = useApplicationDispatch();
    const { isModalShow } = useApplicationState();
    const profileData = useUserState();
    const { id: applicationId } = useParams();
    const navigate = useNavigate();
    const queryCache = useQueryCache();
    const [errorCode, setErrorCode] = useState<null | string>(null);
    const [pollingTime, setPollingTime] = useState<Date | null>(null);
    const [refetchInterval, setRefechInterval] = useState<number | false | undefined>(false);
    const [validationContex, setValidationContext] = useState({
        isLoanForStudent: false,
        loanLimits: null
    });

    const formMethods = useForm<IApplication>({
        resolver: yupResolver(applicationBaseScheme),
        context: validationContex
    });
    const { handleSubmit, reset, watch, getValues } = formMethods;

    const loanTypeId = watch(`LOAN_TYPE_ID`);
    const currencyCode = watch(`CURRENCY_CODE`);
    const agreedWithTerm = watch('AGREED_WITH_TERMS');

    // useQueryies
    const { data: applicationData, error, updatedAt } = useApplication(applicationId, {
        refetchInterval,
        enabled: !!refetchInterval
    });

    const { data: loanLimits } = useLoanLimits(loanTypeId, currencyCode);

    const statusID = applicationData?.STATUS_ID;

    const startPolling = () => {
        setRefechInterval(10000);
        setPollingTime(new Date());
        setModalState(true);
        queryCache.invalidateQueries(['APPLICATIONS']);
    };

    // mutations
    const [mutate, { isLoading: isApplicationSubmitting }] = useMutation(saveApplication, {
        onSuccess: ({ data }) => {
            queryCache.removeQueries('APPLICATION');
            navigate(`/application/${data}`, { replace: true });
            startPolling();
        },
        onError: (err: any) => {
            if (err && err.ErrorCode) {
                setErrorCode(err.ErrorCode as string);
                setModalState(true);
            }
        }
    });

    const onSubmit = useCallback(
        (values: IApplication) => {
            mutate({ ...values, ID: applicationId, SUBMIT: true });
        },
        [applicationId, mutate]
    );

    const handleSave = async () => {
        const formData = getValues();
        const { data } = await saveApplication({
            ...formData,
            SUBMIT: false,
            ORGANIZATION_ACTIVITY_CODE: ''
        });
        if (data) {
            navigate(`/application/${data}`, { replace: true });
        }
    };

    const handleCloseModal = () => {
        setModalState(false);
        setRefechInterval(false);
    };

    const handleContinueRequest = () => {
        setModalState(false);
        setActiveTabName('DETAILS');
    };

    const handleIsloanForStudentChange = useCallback((isLoanForStudent: boolean) => {
        setValidationContext((prev: any) => ({ ...prev, isLoanForStudent }));
    }, []);

    useEffect(() => {
        if (loanLimits) {
            setValidationContext((prev: any) => ({ ...prev, loanLimits }));
        }
    }, [loanLimits]);

    useEffect(() => {
        const initialValues = {};

        if (profileData) {
            Object.assign(initialValues, {
                FIRST_NAME: profileData.FIRST_NAME,
                LAST_NAME: profileData.LAST_NAME,
                PATRONYMIC_NAME: profileData.PATRONYMIC_NAME,
                BIRTH_DATE: profileData.BIRTH_DATE,
                ONBOARDING_ID: profileData.ONBOARDING_ID,
                ...getIdentityFromProfile(profileData)
            });
        }

        if (applicationData) {
            Object.assign(initialValues, {
                ...applicationData,
                AGREED_WITH_TERMS: !!applicationId
            });
        }

        reset(initialValues);
    }, [reset, profileData, applicationData, applicationId]);

    useEffect(() => {
        if (error && error.ErrorCode) {
            setErrorCode(error.ErrorCode as string);
        }
    }, [error]);

    useEffect(() => {
        if (pollingTime && updatedAt) {
            const diff = differenceInMinutes(new Date(updatedAt), pollingTime);
            // is polling applicatoin more then 3 minute
            if (diff >= 3) {
                setErrorCode('timeout');
            }
        }
    }, [pollingTime, updatedAt]);

    useEffect(() => {
        if (!!errorCode || (statusID && [5, 6, 7].includes(statusID))) {
            setRefechInterval(false);
        }
    }, [errorCode, statusID]);

    return (
        <FieldSet className="application-form-wrapper" disabled={disabled}>
            <FormProvider {...formMethods}>
                <ApplicationBaseForm
                    onSubmit={handleSubmit(onSubmit)}
                    isTabDisabled={disabled}
                    onIsLoanForStudentChange={handleIsloanForStudentChange}
                />

                {!disabled && (
                    <div className="form-actions">
                        <LoadingButton
                            variant="outline-primary"
                            loading={false}
                            onClick={handleSave}
                        >
                            Հիշել
                        </LoadingButton>

                        <LoadingButton
                            type="submit"
                            variant="primary"
                            form="ApplicationBaseForm"
                            loading={isApplicationSubmitting}
                            disabled={!agreedWithTerm}
                        >
                            Շարունակել
                        </LoadingButton>
                    </div>
                )}
            </FormProvider>
            <Modal show={isModalShow} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Կատարվում է հարցում</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <RequestModalContent statusId={statusID} errorCode={errorCode} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="outline-primary" as={Link} to="/">
                        Վերադառնալ գլխավոր էջ
                    </Button>
                    {statusID === 5 && (
                        <Button variant="primary" onClick={handleContinueRequest}>
                            Շարունակել
                        </Button>
                    )}
                </Modal.Footer>
            </Modal>
        </FieldSet>
    );
}

export default React.memo(ApplicationBase);
