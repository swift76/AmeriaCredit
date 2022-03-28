import React from 'react';
import NumberFormat, { NumberFormatValues } from 'react-number-format';
import { TextField } from 'components/Form';

// TODO: fix any types of props

function NumberFormatField({ onChange, setValue, ...rest }: any) {
    const onValueChange = (values: NumberFormatValues) => {
        setValue(rest.name, values.value, { shouldValidate: true });
    };

    return (
        <NumberFormat
            thousandSeparator=","
            isNumericString
            customInput={TextField}
            onValueChange={onValueChange}
            {...rest}
        />
    );
}

export default NumberFormatField;
