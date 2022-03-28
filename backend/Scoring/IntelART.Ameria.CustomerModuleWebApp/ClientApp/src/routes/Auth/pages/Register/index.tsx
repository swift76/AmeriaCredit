import React, { useEffect, useState, useCallback } from 'react';
import { useAuthState } from 'providers/AuthProvider';
import RegisterForm from './components/RegisterForm';
import VerifyRegisterForm from './components/VerifyRegisterForm';

function Register() {
    const { processID } = useAuthState();
    const [verifyView, setVerifyView] = useState<boolean>(false);

    useEffect(() => {
        setVerifyView(!!processID);
    }, [processID]);

    return <div className="signup">{verifyView ? <VerifyRegisterForm /> : <RegisterForm />}</div>;
}

export default Register;
