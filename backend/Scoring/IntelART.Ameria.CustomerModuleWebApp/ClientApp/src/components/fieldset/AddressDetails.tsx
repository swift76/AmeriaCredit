import React, { useState, useMemo } from 'react';
import { Row, Col, Form } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';
import { ADDRESS_DETAILS_NAMES as NAMES } from 'constants/application';
import { ValuesOf } from 'types';
import { useCountries, useStates, useCities } from 'hooks';
import { buildSelectOption } from 'helpers/data';
import { TextField, SelectField } from 'components/Form';

type Props = {
    title: string;
    namePrefix: 'REGISTRATION' | 'CURRENT';
    isSameOption?: boolean;
    onIsSame?: (checked: boolean) => void;
    readOnly?: boolean;
};
type NamesObj = {
    [k in ValuesOf<typeof NAMES>]: string;
};

function AddressDetails({
    title,
    namePrefix: prefix,
    isSameOption = false,
    onIsSame,
    readOnly
}: Props) {
    const { register, errors, watch, control, setValue, getValues } = useFormContext();

    const namesMap = useMemo<NamesObj>(
        () => NAMES.reduce<NamesObj>((a, n) => ({ ...a, [n]: `${prefix}_${n}` }), {}),
        [prefix]
    );
    const countryCode = watch(namesMap.COUNTRY_CODE);
    const stateCode = watch(namesMap.STATE_CODE);

    const { isLoading: isCountriesLoading, data: countriesData } = useCountries();
    const { isLoading: isStatesLoading, data: statesData } = useStates();
    const [isCurrentAddressSame, setIsCurrentAddressSame] = useState<boolean>(false);
    const { isLoading: isCitiesLoading, data: citiesData } = useCities(stateCode, {
        enabled: !!stateCode
    });

    const countriesOptions = useMemo(() => {
        return buildSelectOption(countriesData || []);
    }, [countriesData]);

    const statesOptions = useMemo(() => {
        return buildSelectOption(statesData || []);
    }, [statesData]);

    const citiesOptions = useMemo(() => {
        return citiesData?.length ? buildSelectOption(citiesData) : [];
    }, [citiesData]);

    const handleChangeIsSame = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { checked } = event.target;
        if (checked) {
            NAMES.forEach(code => {
                setValue(`CURRENT_${code}`, getValues(`REGISTRATION_${code}`), {
                    shouldValidate: true
                });
            });
        }
        setIsCurrentAddressSame(checked);
    };

    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">{title}</h2>
            </Col>
            {isSameOption && (
                <>
                    <Col sm={12}>
                        <Form.Group controlId="isSameFieldSet">
                            <Form.Check
                                type="checkbox"
                                label="????????, ?????? ?????? ???????????????? ?? ???????????????????? ?????????????????? ???????????????????? ????"
                                onChange={handleChangeIsSame}
                                id="isSameFieldSet"
                            />
                        </Form.Group>
                    </Col>
                </>
            )}
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name={namesMap.COUNTRY_CODE}
                    error={errors?.[namesMap.COUNTRY_CODE]}
                    label="??????????"
                    loading={isCountriesLoading}
                    options={countriesOptions}
                    disabled={isCurrentAddressSame}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name={namesMap.STATE_CODE}
                    label="????????"
                    options={statesOptions}
                    loading={isStatesLoading}
                    error={errors?.[namesMap.STATE_CODE]}
                    disabled={!countryCode || isCurrentAddressSame}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name={namesMap.CITY_CODE}
                    label="??????????????????"
                    options={citiesOptions}
                    loading={isCitiesLoading}
                    error={errors?.[namesMap.CITY_CODE]}
                    disabled={!stateCode || isCurrentAddressSame}
                />
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name={namesMap.STREET}
                    label="??????????"
                    error={errors?.[namesMap.STREET]}
                    readOnly={isCurrentAddressSame}
                />
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name={namesMap.BUILDNUM}
                    label="???????? / ????????"
                    error={errors?.[namesMap.BUILDNUM]}
                    readOnly={isCurrentAddressSame}
                />
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name={namesMap.APARTMENT}
                    label="????????????????"
                    error={errors?.[namesMap.APARTMENT]}
                    readOnly={isCurrentAddressSame}
                />
            </Col>
        </Row>
    );
}

export default AddressDetails;
