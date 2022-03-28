import React, { useMemo } from 'react';
import DatePicker from 'react-datepicker';
import { TextField } from 'components/Form';
import { toUtcMidnight } from 'helpers/date';
import { FormInputProps } from './types';

function DatePickerInput(props: FormInputProps & { minDate?: Date; maxDate?: Date }, ref: any) {
    const { value = null, setValue, readOnly, minDate, maxDate, ...rest } = props;

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
            minDate={minDate}
            maxDate={maxDate}
            onChange={handleChange}
            placeholderText="օր/ամիս/տարի"
            dateFormat="dd/MM/yyyy"
            customInput={<TextField ref={ref} className="datepicker-form-control" {...rest} />}
            readOnly={readOnly}
        />
    );
}

export default React.forwardRef(DatePickerInput);
