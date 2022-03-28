import React from 'react';
import { Row, Col } from 'react-bootstrap';
import { Controller, useFormContext } from 'react-hook-form';
import { TextField, RestrictedInput, DatePickerInput, PhoneNumberInput } from 'components/Form';
import { NameSurName, Citizenship } from 'components/fieldset';
import { onlyAm } from 'validators/regexp';

type Props = {
    nameEN?: boolean;
    nameAM?: boolean;
    patronicName?: boolean;
    mobilePhone?: boolean;
    fixedPhone?: boolean;
    citizenShip?: boolean;
    birthDate?: boolean;
    email?: boolean;
    isDataOnBoard?: boolean;
};

function PersonalDetails(props: Props) {
    const {
        nameAM,
        nameEN,
        mobilePhone,
        fixedPhone,
        citizenShip,
        birthDate,
        patronicName,
        email,
        isDataOnBoard
    } = props;
    const { register, errors, control, setValue } = useFormContext();

    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">Անձնական տվյալներ</h2>
            </Col>

            {nameEN && (
                <NameSurName
                    isEnglish
                    withRow={false}
                    key="english"
                    firstName="FIRST_NAME_EN"
                    lastName="LAST_NAME_EN"
                    readOnly={isDataOnBoard}
                />
            )}
            {nameAM && (
                <NameSurName
                    isEnglish={false}
                    withRow={false}
                    key="armenian"
                    firstName="FIRST_NAME"
                    lastName="LAST_NAME"
                    readOnly={isDataOnBoard}
                />
            )}

            {patronicName && (
                <Col sm={6}>
                    <Controller
                        name="PATRONYMIC_NAME"
                        as={RestrictedInput}
                        control={control}
                        error={errors.PATRONYMIC_NAME}
                        label="Հայրանուն (հայերեն)"
                        regexp={onlyAm}
                        readOnly={isDataOnBoard}
                    />
                </Col>
            )}

            {birthDate && (
                <Col sm={6}>
                    <Controller
                        as={DatePickerInput}
                        control={control}
                        name="BIRTH_DATE"
                        error={errors.BIRTH_DATE}
                        withGroup={false}
                        placeholder="օր/ամիս/տարի"
                        label="Ծննդյան ամսաթիվ"
                        setValue={setValue}
                        readOnly={isDataOnBoard}
                    />
                </Col>
            )}

            {citizenShip && <Citizenship withRow={false} />}

            {mobilePhone && (
                <Col sm={6}>
                    <PhoneNumberInput
                        name="MOBILE_PHONE"
                        control={control}
                        error={errors.MOBILE_PHONE}
                        readOnly
                    />
                </Col>
            )}

            {fixedPhone && (
                <Col sm={6}>
                    <PhoneNumberInput
                        name="FIXED_PHONE"
                        label="Քաղաքային հեռախոս"
                        control={control}
                        error={errors.FIXED_PHONE}
                        readOnly={isDataOnBoard}
                    />
                </Col>
            )}

            {email && (
                <Col sm={6}>
                    <TextField
                        name="EMAIL"
                        ref={register}
                        error={errors.EMAIL}
                        label="Էլ․ հասցե"
                        type="text"
                        readOnly={isDataOnBoard}
                    />
                </Col>
            )}
        </Row>
    );
}

export default PersonalDetails;
