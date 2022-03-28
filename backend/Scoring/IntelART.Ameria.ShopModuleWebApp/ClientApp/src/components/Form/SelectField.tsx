import React from 'react';
import clsx from 'clsx';
import { ISelectOption } from 'types';
import TextField from './TextField';
import { FormInputProps } from './types';

type Props = FormInputProps & {
    options: ISelectOption[];
    loading?: boolean;
    noChoiceOption?: boolean;
};

function SelectField(props: Props) {
    const {
        value: initialValue,
        regexp,
        onChange,
        loading,
        noChoiceOption,
        options,
        ref,
        ...rest
    } = props;

    return (
        <TextField
            onChange={onChange}
            value={initialValue || ''}
            custom
            className={clsx(loading && 'is-loading')}
            as="select"
            ref={null}
            {...rest}
        >
            {!noChoiceOption && <option value="">Ընտրել</option>}
            {options.map(({ value, name }) => (
                <option value={value} key={value}>
                    {name}
                </option>
            ))}
        </TextField>
    );
}

export default SelectField;
