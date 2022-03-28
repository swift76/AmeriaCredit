import React from 'react';
import { Form, InputGroup } from 'react-bootstrap';
import { Controller, Control, FieldError } from 'react-hook-form';
import { RestrictedInput } from 'components/Form';

type Props = {
    name?: string;
    label?: string;
    error?: FieldError;
    control: Control<Record<string, any>>;
    autoComplete?: string;
    rules?: Record<string, any>;
    disabled?: boolean;
    readOnly?: boolean;
};

function PhoneNumberInput({
    name = `MOBILE_PHONE`,
    label = `Բջջային հեռախոս`,
    error,
    control,
    readOnly,
    ...rest
}: Props) {
    return (
        <Form.Group controlId={`${name}_Control`}>
            <Form.Label>{label}</Form.Label>
            <InputGroup>
                <InputGroup.Prepend>
                    <InputGroup.Text>374</InputGroup.Text>
                </InputGroup.Prepend>
                <Controller
                    as={RestrictedInput}
                    control={control}
                    name={name}
                    type="number"
                    error={error}
                    withGroup={false}
                    placeholder="XX XXX XXX"
                    maxLength={8}
                    readOnly={readOnly}
                    {...rest}
                />
            </InputGroup>
        </Form.Group>
    );
}

export default PhoneNumberInput;
