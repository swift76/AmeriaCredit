import React, { useMemo } from 'react';
import DatePicker from 'react-datepicker';
import { TextField } from 'components/Form';
import { toUtcMidnight } from 'helpers/date';
import { FormInputProps } from './types';

function DatePickerInput(props: FormInputProps, ref: any) {
    const { value = null, setValue, readOnly, ...rest } = props;

    const selected = useMemo(() => {
        return value ? new Date(value.toString()) : null;
    }, [value]);

    const handleChange = (date: Date) => {
        if (rest?.name && setValue) {
            setValue(rest.name, toUtcMidnight(date));
        }
    };

    return (
        <DatePicker
            selected={selected}
            onChange={handleChange}
            placeholderText="օր/ամիս/տարի"
            dateFormat="dd/MM/yyyy"
            customInput={<TextField ref={ref} className="datepicker-form-control" {...rest} />}
            readOnly={readOnly}
        />
    );
}

export default React.forwardRef(DatePickerInput);
