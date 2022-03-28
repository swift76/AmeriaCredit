import React from 'react';
import { Badge } from 'react-bootstrap';
import clsx from 'clsx';
import { pendingStatuses, rejectedStatuses, approvedStatuses } from 'constants/application';

type Props = {
    status: string;
    children: React.ReactNode;
};
export default function StatusBadge({ status, children }: Props) {
    return (
        <Badge
            variant={clsx({
                pending: pendingStatuses.includes(status),
                approved: approvedStatuses.includes(status),
                rejected: rejectedStatuses.includes(status)
            })}
        >
            {children}
        </Badge>
    );
}
