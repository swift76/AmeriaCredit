import React, { useMemo, Fragment } from 'react';
import { Row, Col } from 'react-bootstrap';
import { SelectField } from 'components/Form';
import { useFormContext, Controller } from 'react-hook-form';
import { useCountries } from 'hooks';
import { buildSelectOption } from 'helpers/data';

type Props = {
    withRow?: boolean;
};

function Citizenship({ withRow }: Props) {
    const FieldsRow: Row | typeof Fragment = withRow ? Row : Fragment;
    const { errors, control } = useFormContext();
    const { isLoading: isCountriesLoading, data: countriesData } = useCountries();

    if (FieldsRow === Row) {
        (FieldsRow as Row).defaultProps = {
            className: 'mb-3'
        };
    }

    const countriesOptions = useMemo(() => {
        return buildSelectOption(countriesData || []);
    }, [countriesData]);

    return (
        <FieldsRow>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name="BIRTH_PLACE_CODE"
                    label="Ծննդավայր"
                    error={errors.BIRTH_PLACE_CODE}
                    options={countriesOptions}
                    loading={isCountriesLoading}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name="CITIZENSHIP_CODE"
                    label="Քաղաքացիություն"
                    error={errors.CITIZENSHIP_CODE}
                    options={countriesOptions}
                    loading={isCountriesLoading}
                />
            </Col>
        </FieldsRow>
    );
}

export default Citizenship;
