import React from 'react';
import { TextField } from 'components/Form';
import { FormControlElement, FormInputProps } from './types';

function RestrictedInput(props: FormInputProps) {
    const { value: val, regexp, onChange, ref, ...rest } = props;

    const handleChange = (event: React.ChangeEvent<FormControlElement>) => {
        const { maxLength } = rest;
        const { value, type } = event.target;
        if (
            !onChange ||
            (type === 'number' && maxLength && value.length > maxLength) ||
            (regexp && !regexp.test(value))
        ) {
            return false;
        }
        return onChange(event);
    };

    return <TextField onChange={handleChange} value={val || ''} {...rest} ref={null} />;
}

export default RestrictedInput;
