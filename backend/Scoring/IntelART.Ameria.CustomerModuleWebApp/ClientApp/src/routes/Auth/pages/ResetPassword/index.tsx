import React, { useEffect } from 'react';
import { useAuthState, useAuthDispatch } from 'providers/AuthProvider';
import NewPasswordForm from './components/NewPasswordForm';
import ResePasswordForm from './components/ResePasswordForm';

function ResetPassword() {
    const { resetAuthState } = useAuthDispatch();
    const { processID, userPhone } = useAuthState();

    useEffect(() => {
        resetAuthState();
    }, [resetAuthState]);

    return <div className="signup">{processID ? <NewPasswordForm /> : <ResePasswordForm />}</div>;
}

export default ResetPassword;
