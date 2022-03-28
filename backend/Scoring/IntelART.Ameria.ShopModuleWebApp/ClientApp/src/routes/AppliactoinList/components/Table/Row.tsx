import React, { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { IApplications } from 'types';
import { format } from 'date-fns';
import clsx from 'clsx';
import { DATE_FORMAT_DEFAULT } from 'constants/date';
import {
    APPLICATION_STATUS_ENUM as statuses,
    pendingStatuses,
    rejectedStatuses,
    approvedStatuses
} from 'constants/application';
import ActionCell from './ActionCell';

export interface Props {
    data: IApplications;
    rowProps: any;
}

function TableRow({ data, rowProps }: Props) {
    const navigate = useNavigate();
    const { ID, STATUS_STATE } = data;

    const handleRowClick = useCallback(() => {
        if (STATUS_STATE !== statuses.APPROVAL_REVIEW) {
            navigate(`/application/${ID}`);
        }
    }, [navigate, ID, STATUS_STATE]);

    return (
        <tr onClick={handleRowClick} {...rowProps}>
            <td>{data.NAME}</td>
            <td>{format(new Date(data.CREATION_DATE), DATE_FORMAT_DEFAULT)}</td>
            <td>{data.DISPLAY_AMOUNT}</td>
            <td>
                <span
                    className={clsx('line2-text', {
                        'text-pending': pendingStatuses.includes(data.STATUS_STATE),
                        'text-approved': approvedStatuses.includes(data.STATUS_STATE),
                        'text-rejected': rejectedStatuses.includes(data.STATUS_STATE)
                    })}
                >
                    {data.STATUS_AM}
                </span>
            </td>
            <td className="text-center">
                <ActionCell id={ID} status={data.STATUS_STATE} onboardId={data.ONBOARDING_ID} />
            </td>
        </tr>
    );
}

export default TableRow;
