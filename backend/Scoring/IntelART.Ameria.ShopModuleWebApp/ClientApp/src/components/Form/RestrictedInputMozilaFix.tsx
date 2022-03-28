import React, { useState, useEffect } from 'react';
import { TextField } from 'components/Form';
import { FormControlElement, FormInputProps } from './types';

function RestrictedInput(props: FormInputProps) {
    const { value: val, regexp, type: inputType, onChange, ref, ...rest } = props;
    const [inputValue, setInputValue] = useState(val);

    const handleChange = (event: React.ChangeEvent<FormControlElement>) => {
        const { maxLength } = rest;
        const { value, type } = event.target;

        // eslint-disable-next-line no-restricted-globals
        if (inputType === 'number' && isNaN(Number(value))) return false;
        if (
            !onChange ||
            (type === 'number' && maxLength && value.length > maxLength) ||
            (regexp && !regexp.test(value))
        ) {
            return false;
        }
        return onChange(event);
    };

    useEffect(() => {
        setInputValue(val);
    }, [setInputValue, val]);

    useEffect(() => {
        if (inputType === 'number') setInputValue('');
    }, [setInputValue, inputType]);

    return (
        <TextField
            onChange={handleChange}
            value={inputValue || ''}
            {...rest}
            ref={null}
            type={inputType !== 'number' ? inputType : ''}
        />
    );
}

export default RestrictedInput;
