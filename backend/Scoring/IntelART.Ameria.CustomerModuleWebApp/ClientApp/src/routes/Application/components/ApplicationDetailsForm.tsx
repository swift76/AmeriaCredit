import React, { useState, useMemo, useCallback, useEffect } from 'react';
import { Form, Row, Col, Button } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';
import { buildSelectOption, getDurationOptions, getRepayDayValue } from 'helpers/data';
import {
    useApplication,
    useApplicationStatusText,
    useLoanTypes,
    useMaritalStatuses,
    useLoanParameters
} from 'hooks';
import { SelectField, RestrictedInput, NumberFormatField } from 'components/Form';
import { PersonalDetails, AddressDetails, WorkingDetails, Documents } from 'components/fieldset';
import { useUserState } from 'providers/UserProvider';
import { getRepaymentSchedule } from 'queryies';
import { ISelectOption, IScoringResults } from 'types';
import RefinancingTable from './RefinancingTable';
import StatusBadge from './StatusBadge';

type Props = {
    scoringResult?: IScoringResults[];
    interest?: IScoringResults;
    id: string;
    onSubmit: (e?: React.BaseSyntheticEvent<object, any, any> | undefined) => Promise<void>;
    isTabDisabled: boolean;
    disableRepayDay?: boolean;
};

function ApplicationDetailsForm({
    onSubmit,
    id,
    scoringResult,
    interest,
    isTabDisabled,
    disableRepayDay = false
}: Props) {
    const [monthlyAmount, setMonthlyAmount] = useState<number | null>(null);
    const [durationOptions, setDurationOptions] = useState<ISelectOption[]>([]);
    const { watch, control, errors, setValue } = useFormContext();
    const profileData = useUserState();

    const { data: maritalStatuses, isLoading: isMaritalStatusesLoading } = useMaritalStatuses();

    const { data: applicationData } = useApplication(id, {
        enabled: !!id
    });

    const isRefinancing = applicationData?.IS_REFINANCING;
    const loanTypeId = applicationData?.LOAN_TYPE_ID;
    const currencyCode = applicationData?.CURRENCY_CODE;
    const clientCode = applicationData?.CLIENT_CODE?.trim();
    const applicationId = applicationData?.ID;
    const isStudent = profileData?.IS_STUDENT;

    const { data: loanTypes } = useLoanTypes(isStudent, {
        enabled: isStudent !== undefined
    });
    const { statusAm: statusText, statusState } = useApplicationStatusText(applicationId, {
        enabled: !!applicationId
    });

    const { data: loanParameters } = useLoanParameters(loanTypeId, {
        enabled: !!loanTypeId
    });

    const isRepayStartDay = loanParameters?.IS_REPAY_START_DAY ?? false;

    const interestId = watch('INTEREST');
    const amount = watch('AMOUNT');
    const duration = watch('PERIOD_TYPE_CODE');
    const finalAmount = watch('FINAL_AMOUNT');

    const handleCalculateClick = useCallback(async () => {
        if (interestId && amount && duration) {
            const { MONTHLY_PAYMENT_AMOUNT } = await getRepaymentSchedule({
                interest: interestId,
                amount,
                duration
            });
            setMonthlyAmount(MONTHLY_PAYMENT_AMOUNT);
        }
    }, [interestId, amount, duration]);

    const interestOptions = useMemo(() => {
        if (scoringResult?.length) {
            return scoringResult.map(({ INTEREST }) => {
                const value = INTEREST.toString();
                return { value, name: value };
            });
        }
        return [];
    }, [scoringResult]);

    const maritalStatusesOptions = useMemo(() => {
        return buildSelectOption(maritalStatuses);
    }, [maritalStatuses]);

    useEffect(() => {
        if (interest && loanTypes?.length && loanTypeId) {
            const options = getDurationOptions(interest, loanTypes, loanTypeId);
            setDurationOptions(options);
        }
    }, [interest, loanTypes, loanTypeId]);

    useEffect(() => {
        if (interest) {
            setValue('AMOUNT', interest.AMOUNT, { shouldValidate: true });
        }
    }, [setValue, interest]);

    useEffect(() => {
        if (finalAmount) {
            setValue('AMOUNT', finalAmount, { shouldValidate: true });
        }
    }, [setValue, finalAmount]);

    useEffect(() => {
        if (loanTypeId && disableRepayDay) {
            setValue('REPAY_DAY', getRepayDayValue(loanTypes, loanTypeId, isRepayStartDay));
        }
    }, [disableRepayDay, isRepayStartDay, loanTypeId, loanTypes, setValue]);

    return (
        <Form onSubmit={onSubmit} id="ApplicationDetailsForm">
            <Row className="mb-3">
                <Col sm={12}>
                    <h2 className="fieldset-title">
                        Վարկի տվյալներ{' '}
                        {isTabDisabled && (
                            <StatusBadge status={statusState ?? ''}>{statusText}</StatusBadge>
                        )}
                    </h2>
                </Col>
                <Col sm={6}>
                    <Controller
                        as={SelectField}
                        control={control}
                        name="INTEREST"
                        label="Տոկոսադրույք"
                        error={errors.INTEREST}
                        options={interestOptions}
                    />
                </Col>
                <Col sm={6}>
                    <Controller
                        as={SelectField}
                        control={control}
                        name="PERIOD_TYPE_CODE"
                        label="Ժամկետ"
                        error={errors.PERIOD_TYPE_CODE}
                        options={durationOptions}
                    />
                </Col>
                <Col sm={6}>
                    <Controller
                        as={NumberFormatField}
                        control={control}
                        name="AMOUNT"
                        label="Առաջարկվող գումար"
                        error={errors.AMOUNT}
                        setValue={setValue}
                        readOnly
                    />
                </Col>
                <Col sm={6}>
                    <Controller
                        name="REPAY_DAY"
                        as={RestrictedInput}
                        control={control}
                        error={errors.REPAY_DAY}
                        label="Նախընտրելի մարման օր *"
                        type="number"
                        maxLength={2}
                        readOnly={disableRepayDay}
                    />
                </Col>
                <Col sm={6} className="mb-3">
                    <Button
                        variant="primary"
                        onClick={handleCalculateClick}
                        disabled={
                            duration === '0' || amount === '0' || !amount || !duration || !interest
                        }
                    >
                        Հաշվարկել ամսական վճարի չափը
                    </Button>
                </Col>
                {!!monthlyAmount && (
                    <Col sm={12} className="mt-4">
                        <div className="alert alert-success">
                            {`Ձեր ամսական վճարի մոտավոր չափը կազմում է ${monthlyAmount} դրամ`}
                        </div>
                    </Col>
                )}

                {isRefinancing && <RefinancingTable id={id} />}
            </Row>

            <PersonalDetails nameEN citizenShip mobilePhone fixedPhone email />

            <AddressDetails title="Գրանցման հասցե" namePrefix="REGISTRATION" />

            <AddressDetails title="Բնակության հասցե" namePrefix="CURRENT" isSameOption />

            <Row className="mb-3">
                <Col sm={12}>
                    <h2 className="fieldset-title">Ընտանիքի տվյալներ</h2>
                </Col>
                <Col sm={6}>
                    <Controller
                        as={SelectField}
                        control={control}
                        name="FAMILY_STATUS_CODE"
                        label="Ընտանեկան դրություն *"
                        error={errors.FAMILY_STATUS_CODE}
                        options={maritalStatusesOptions}
                        loading={isMaritalStatusesLoading}
                    />
                </Col>
            </Row>
            {!clientCode && (
                <>
                    <WorkingDetails />
                    <Documents id={id} />
                </>
            )}
        </Form>
    );
}

export default ApplicationDetailsForm;
