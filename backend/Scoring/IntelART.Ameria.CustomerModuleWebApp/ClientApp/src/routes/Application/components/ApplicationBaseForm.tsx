/* eslint-disable react/no-danger */
import React, { useEffect, useMemo } from 'react';
import { Form, Row, Col } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';
import { useParams } from 'react-router-dom';
import { SelectField, NumberFormatField } from 'components/Form';
import IdentityDocuments from 'components/fieldset/IdentityDocuments';
import PersonalDetails from 'components/fieldset/PersonalDetails';
import { UniversityDetails } from 'components/fieldset';

import {
    useLoanTypes,
    useIndustries,
    useCurrencies,
    useLoanLimits,
    useApplicationStatusText
} from 'hooks';
import { LOAN_HINTS } from 'constants/application';
import { ISelectOption, ILoanTypes } from 'types';
import { buildSelectOption, getAmountLimitPlaceholder } from 'helpers/data';
import { useUserState } from 'providers/UserProvider';
import AgreedWithTermsLabel from './AgreedWithTermsLabel';
import StatusBadge from './StatusBadge';

type Props = {
    onSubmit: (e?: React.BaseSyntheticEvent<object, any, any> | undefined) => Promise<void>;
    isTabDisabled: boolean;
    onIsLoanForStudentChange: (isLoanForStudent: boolean) => void;
};

function ApplicationBaseForm({ onSubmit, isTabDisabled, onIsLoanForStudentChange }: Props) {
    const { register, control, errors, setValue, watch } = useFormContext();
    const { id: applicationId } = useParams();
    const profileData = useUserState();
    const loanTypeId = watch(`LOAN_TYPE_ID`);
    const currencyCode = watch(`CURRENCY_CODE`);

    const isStudent = profileData?.IS_STUDENT;

    // useQuery
    const { data: loanTypesData, isLoading: isLoanTypesLoading } = useLoanTypes(isStudent, {
        enabled: isStudent !== undefined
    });
    const { data: industriesData, isLoading: isIndustriesLoading } = useIndustries();
    const { data: currenciesData, isLoading: isCurrenciesLoading } = useCurrencies(loanTypeId);
    const { data: loanLimits } = useLoanLimits(loanTypeId, currencyCode);
    const { statusAm: statusText, statusState } = useApplicationStatusText(applicationId, {
        enabled: !!applicationId
    });

    const isLoanTypeForStudent = useMemo<boolean>(() => {
        if (loanTypesData && loanTypeId) {
            return (
                loanTypesData.find((loanType: ILoanTypes) => loanType.CODE === loanTypeId)
                    ?.IS_STUDENT ?? false
            );
        }
        return false;
    }, [loanTypeId, loanTypesData]);

    const loanTypesOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOption(loanTypesData);
    }, [loanTypesData]);

    const industriesOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOption(industriesData);
    }, [industriesData]);

    const currenciesOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOption(currenciesData);
    }, [currenciesData]);

    const amountPlaceholder = useMemo(() => {
        if (loanLimits) {
            return getAmountLimitPlaceholder(loanLimits);
        }
        return '';
    }, [loanLimits]);

    useEffect(() => {
        onIsLoanForStudentChange(isLoanTypeForStudent);
    }, [isLoanTypeForStudent, onIsLoanForStudentChange]);

    return (
        <Form onSubmit={onSubmit} id="ApplicationBaseForm">
            <Row className="mb-3">
                <Col sm={12}>
                    <h2 className="fieldset-title">
                        Նոր հայտ{' '}
                        {isTabDisabled && (
                            <StatusBadge status={statusState ?? ''}>{statusText}</StatusBadge>
                        )}
                    </h2>
                </Col>
                <Col sm={12}>
                    <Controller
                        as={SelectField}
                        control={control}
                        name="LOAN_TYPE_ID"
                        label="Վարկի տեսակ *"
                        error={errors.LOAN_TYPE_ID}
                        options={loanTypesOptions}
                        loading={isLoanTypesLoading}
                    />
                    <div
                        dangerouslySetInnerHTML={{ __html: (LOAN_HINTS as any)[loanTypeId] }}
                        className="form-label"
                    />
                </Col>
            </Row>
            <Row className="mb-3">
                <Col sm={12}>
                    <h2 className="fieldset-title">Վարկի տվյալներ</h2>
                </Col>
                <Col sm={6}>
                    <Controller
                        as={SelectField}
                        control={control}
                        name="CURRENCY_CODE"
                        label="Արժույթ *"
                        error={errors.CURRENCY_CODE}
                        options={currenciesOptions}
                        loading={isCurrenciesLoading}
                    />
                </Col>
                <Col sm={6}>
                    <Controller
                        as={NumberFormatField}
                        name="INITIAL_AMOUNT"
                        control={control}
                        error={errors.INITIAL_AMOUNT}
                        label="Գումար *"
                        placeholder={amountPlaceholder}
                        setValue={setValue}
                    />
                </Col>
                {!isLoanTypeForStudent && (
                    <Col sm={12}>
                        <Form.Group controlId="isRefinancingControl">
                            <Form.Check
                                type="checkbox"
                                ref={register}
                                label="Վերաֆինանսավորում"
                                name="IS_REFINANCING"
                            />
                        </Form.Group>
                    </Col>
                )}
            </Row>

            <PersonalDetails nameAM patronicName birthDate />

            <IdentityDocuments />

            <Row className="mb-3">
                <Col sm={12}>
                    <h2 className="fieldset-title">Գործունեության ոլորտ</h2>
                </Col>
                <Col sm={6}>
                    <Controller
                        as={SelectField}
                        control={control}
                        name="ORGANIZATION_ACTIVITY_CODE"
                        label="Գործունեության ոլորտ *"
                        error={errors.ORGANIZATION_ACTIVITY_CODE}
                        options={industriesOptions}
                        loading={isIndustriesLoading}
                    />
                </Col>
            </Row>

            {isLoanTypeForStudent && <UniversityDetails />}

            <Row className="mb-3">
                <Col sm={12}>
                    <Form.Group controlId="AgreedWithTermsControl">
                        <Form.Check
                            type="checkbox"
                            ref={register}
                            label={<AgreedWithTermsLabel />}
                            name="AGREED_WITH_TERMS"
                        />
                    </Form.Group>
                </Col>
            </Row>
        </Form>
    );
}

export default ApplicationBaseForm;
