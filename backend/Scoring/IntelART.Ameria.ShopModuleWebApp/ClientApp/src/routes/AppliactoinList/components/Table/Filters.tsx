import React, { useMemo } from 'react';
import { Row } from 'react-bootstrap';
import { DatePickerInput, TextField, SelectField } from 'components/Form';
import { APP_TABLE_LENGTH_OPTIONS } from 'constants/applicationsTable';
import clsx from 'clsx';
import { IApplications } from 'types';
import Loading from 'components/Loading';
import { columnsConfig } from './Config';
import TableRow from './Row';

export interface Props {}

function TableFilters() {
    return (
        <Row>
            <span>Ցուցադրել</span>
            <SelectField options={APP_TABLE_LENGTH_OPTIONS} value={10} />
            <span>Գրանցում</span>
            <DatePickerInput
                setValue={(name, date) => console.log(name, date)}
                name="datepicker-from"
            />
            <DatePickerInput name="datepicker-to" />
        </Row>
    );
}

export default TableFilters;
