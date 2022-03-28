import React from 'react';
import { Spinner, Button, ButtonProps } from 'react-bootstrap';

type Props = {
    loading: boolean;
    children: React.ReactNode;
    form?: string;
};

function LoadingButton({ loading, children, ...rest }: Props & ButtonProps) {
    return (
        <Button disabled={loading} {...rest}>
            {loading && <Spinner animation="border" size="sm" />} {children}
        </Button>
    );
}

export default LoadingButton;
