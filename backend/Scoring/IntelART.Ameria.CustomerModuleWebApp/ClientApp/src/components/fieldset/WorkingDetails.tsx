import React, { useMemo } from 'react';
import { Row, Col } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';

import { useWorkExperienceDurations, useMonthlyNetIncomeRanges } from 'hooks/directory';
import { buildSelectOption } from 'helpers/data';
import { TextField, SelectField, PhoneNumberInput } from 'components/Form';

function WorkingDetails() {
    const { register, errors, control } = useFormContext();

    const {
        isLoading: isExperienceDurationsLoading,
        data: workExperiences
    } = useWorkExperienceDurations();

    const workExperiencesOptions = useMemo(() => {
        return buildSelectOption(workExperiences);
    }, [workExperiences]);

    const { isLoading: isMonthlyIncomeLoading, data: monthlyIncome } = useMonthlyNetIncomeRanges();

    const monthlyIncomeOptions = useMemo(() => {
        return buildSelectOption(monthlyIncome);
    }, [monthlyIncome]);

    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">Աշխատանքային տվյալներ</h2>
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name="COMPANY_NAME"
                    label="Գործատուի կազմակեռպության անվանում"
                    error={errors.COMPANY_NAME}
                />
            </Col>
            <Col sm={6}>
                <PhoneNumberInput
                    name="COMPANY_PHONE"
                    control={control}
                    error={errors.COMPANY_PHONE}
                    label="Գործատուի կազմակեռպության հեռախոսահամար"
                />
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name="POSITION"
                    label="Զբաղեցրած պաշտոն"
                    error={errors.POSITION}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name="MONTHLY_INCOME_CODE"
                    error={errors.MONTHLY_INCOME_CODE}
                    label="Ընդհանուր աշխատանքյին փորձ"
                    loading={isExperienceDurationsLoading}
                    options={workExperiencesOptions}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name="WORKING_EXPERIENCE_CODE"
                    error={errors.WORKING_EXPERIENCE_CODE}
                    label="Ամսական զուտ եկամուտ"
                    loading={isMonthlyIncomeLoading}
                    options={monthlyIncomeOptions}
                />
            </Col>
        </Row>
    );
}

export default WorkingDetails;
