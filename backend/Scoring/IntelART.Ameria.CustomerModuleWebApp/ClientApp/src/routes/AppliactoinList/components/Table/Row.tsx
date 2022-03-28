import React, { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { IApplications } from 'types';
import { format } from 'date-fns';
import clsx from 'clsx';
import { DATE_FORMAT_DEFAULT } from 'constants/date';
import { pendingStatuses, rejectedStatuses, approvedStatuses } from 'constants/application';
import ActionCell from './ActionCell';

export interface Props {
    data: IApplications;
}

function TableRow({ data }: Props) {
    const navigate = useNavigate();
    const { ID, STATUS_STATE } = data;

    const handleRowClick = useCallback(() => {
        navigate(`/application/${ID}`);
    }, [navigate, ID]);

    return (
        <tr onClick={handleRowClick}>
            <td>{data.LOAN_TYPE_AM}</td>
            <td>{format(new Date(data.CREATION_DATE), DATE_FORMAT_DEFAULT)}</td>
            <td>{data.DISPLAY_AMOUNT}</td>
            <td>
                <span
                    className={clsx('line2-text', {
                        'text-pending': pendingStatuses.includes(STATUS_STATE),
                        'text-approved': approvedStatuses.includes(STATUS_STATE),
                        'text-rejected': rejectedStatuses.includes(STATUS_STATE)
                    })}
                >
                    {data.STATUS_AM}
                </span>
            </td>
            <td className="text-right">
                <ActionCell id={ID} status={STATUS_STATE} />
            </td>
        </tr>
    );
}

export default TableRow;
