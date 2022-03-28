import React from 'react';
import { Spinner } from 'react-bootstrap';
import clsx from 'clsx';

type Props = {
    className?: string;
};

function Loading({ className = '' }: Props) {
    return (
        <div className={clsx('loading', className)}>
            <Spinner animation="border" role="status" variant="primary">
                <span className="sr-only">Loading...</span>
            </Spinner>
        </div>
    );
}

export default Loading;
