import React from 'react';
import Loading from 'components/Loading';

type Props = {
    disabled?: boolean;
    loading?: boolean;
    children: React.ReactNode;
    className?: string;
};

export default function FieldSet({ disabled, loading, className = '', children }: Props) {
    return (
        <fieldset disabled={disabled} className={`form-fieldset ${className}`}>
            {children}
            {loading && <Loading className="fieldset-loading" />}
        </fieldset>
    );
}
