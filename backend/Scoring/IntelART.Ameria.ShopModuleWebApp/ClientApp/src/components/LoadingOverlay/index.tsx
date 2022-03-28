import React from 'react';

type Props = {
    children: React.ReactNode;
    className?: string;
};

export default function LoadingOverlay({ className = '', children }: Props) {
    return <div className={`loading-overlay ${className}`}>{children}</div>;
}
