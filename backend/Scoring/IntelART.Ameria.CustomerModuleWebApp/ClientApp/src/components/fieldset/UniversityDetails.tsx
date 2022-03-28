import React, { useMemo } from 'react';
import { Row, Col } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';
import { useUniversities } from 'hooks';
import { buildSelectOption } from 'helpers/data';
import { TextField, SelectField, RestrictedInput } from 'components/Form';
import { onlyAm } from 'validators/regexp';

function UniversityDetails() {
    const { register, errors, control } = useFormContext();
    const { data: universities, isLoading: isUniversitiesLoading } = useUniversities();

    const universitiesOptions = useMemo(() => {
        return buildSelectOption(universities);
    }, [universities]);
    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">Համալսարանական տվյալներ</h2>
            </Col>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name="UNIVERSITY_CODE"
                    error={errors.UNIVERSITY_CODE}
                    label="Համալսարան"
                    loading={isUniversitiesLoading}
                    options={universitiesOptions}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={RestrictedInput}
                    control={control}
                    name="UNIVERSITY_FACULTY"
                    label="ֆակուլտետ (հայերեն)"
                    error={errors.UNIVERSITY_FACULTY}
                    regexp={onlyAm}
                />
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name="UNIVERSITY_YEAR"
                    label="Կուրս"
                    error={errors.UNIVERSITY_YEAR}
                />
            </Col>
        </Row>
    );
}

export default UniversityDetails;
