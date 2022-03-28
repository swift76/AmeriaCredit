import React, { useEffect, useState, Fragment } from 'react';
import { Row, Col } from 'react-bootstrap';
import { RestrictedInput } from 'components/Form';
import { Controller, useFormContext } from 'react-hook-form';
import { onlyAm, onlyEng } from 'validators/regexp';

type Props = {
    isEnglish: boolean;
    withRow?: boolean;
    firstName: string;
    lastName: string;
    readOnly?: boolean | undefined;
};

function NameSurName({ isEnglish = false, withRow = true, firstName, lastName, readOnly }: Props) {
    const FieldsRow = withRow ? Row : Fragment;
    const [language, setLanguage] = useState('հայերեն');
    const [regexp, setRegexp] = useState(onlyAm);
    const { errors, control } = useFormContext();

    if (FieldsRow === Row) {
        (FieldsRow as Row).defaultProps = {
            className: 'mb-3'
        };
    }

    useEffect(() => {
        if (isEnglish) {
            setLanguage('անգլերեն');
            setRegexp(onlyEng);
        }
    }, [isEnglish]);

    return (
        <FieldsRow>
            <Col sm={6}>
                <Controller
                    name={firstName}
                    as={RestrictedInput}
                    control={control}
                    error={errors[firstName]}
                    label={`Անուն (${language})`}
                    regexp={regexp}
                    readOnly={readOnly}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    name={lastName}
                    as={RestrictedInput}
                    control={control}
                    error={errors[lastName]}
                    label={`Ազգանուն (${language})`}
                    regexp={regexp}
                    readOnly={readOnly}
                />
            </Col>
        </FieldsRow>
    );
}

export default NameSurName;
