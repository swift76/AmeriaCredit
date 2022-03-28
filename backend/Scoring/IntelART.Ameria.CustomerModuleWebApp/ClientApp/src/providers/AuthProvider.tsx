/* eslint-disable react-hooks/exhaustive-deps */
import React, { createContext, useCallback, useContext, useState, ReactNode } from 'react';
import qs from 'qs';
import Api, { setSession, isValidToken } from 'services/api';
import { BASE_URL as baseUrl } from 'constants/api';
import { toast } from 'react-toastify';

type AuthProviderProps = { children: ReactNode };

type SignInResponse = {
    access_token: string;
    expires_in: number;
    refresh_token: string;
    token_type: string;
};

type SignUpFlowResponse = {
    registrationProcessID?: string;
    phone: string;
    errorMessages: string[];
};

type State = {
    isAutentificated: boolean;
    processID?: string;
    userPhone?: string;
};

const AuthStateContext = createContext<State | undefined>(undefined);
const AuthDispatchContext = createContext<React.Dispatch<React.SetStateAction<State>> | undefined>(
    undefined
);
const initialToken = localStorage.getItem('token');
const validToken = isValidToken(initialToken);

const initialState = {
    isAutentificated: validToken
};

function AuthProvider({ children }: AuthProviderProps) {
    const [state, setState] = useState({ ...initialState });

    return (
        <AuthStateContext.Provider value={state}>
            <AuthDispatchContext.Provider value={setState}>{children}</AuthDispatchContext.Provider>
        </AuthStateContext.Provider>
    );
}

function useAuthState() {
    const state = useContext(AuthStateContext);

    if (state === undefined) {
        throw new Error('useAuthState must be used within a AuthProvider');
    }

    return state;
}

function useAuthDispatch() {
    const setState = useContext(AuthDispatchContext);

    if (setState === undefined) {
        throw new Error('useAuthDispatch must be used within a AUthProvider');
    }

    function processSignUpData(data: SignUpFlowResponse) {
        const { registrationProcessID: processID, phone: userPhone, errorMessages: errors } = data;
        if (setState === undefined) return false;

        if (processID) {
            setState(prev => ({ ...prev, processID, userPhone }));
            return true;
        }
        if (errors.length) {
            toast.error(errors.join(`,`).replace(':', ''));
        } else {
            setState({ isAutentificated: true });
            return true;
        }
        return Promise.reject(errors);
    }

    const setIsAutentificated = useCallback(() => {
        setState({ isAutentificated: true });
    }, [setState]);

    const login = useCallback(async ({ Username, Password }) => {
        try {
            const response = await Api.post<SignInResponse>(
                `${baseUrl}/Account/Login`,
                qs.stringify({ Username, Password })
            );
            setSession(response.data.access_token);

            return response;
        } catch (error: any) {
            if (error && error.error_description) {
                toast.error(error.error_description);
            } else {
                toast.error(`Առկա է տեխնիկական խնդիր`);
            }
            throw error;
        }
    }, []);

    const register = useCallback(async data => {
        const response = await Api.post<SignUpFlowResponse>(
            `${baseUrl}/Account/Register`,
            qs.stringify(data)
        );

        return processSignUpData(response.data);
    }, []);

    const verifyPhone = useCallback(async data => {
        const response = await Api.post<SignUpFlowResponse>(
            `${baseUrl}/Account/VerifyPhone`,
            qs.stringify(data)
        );
        const errors = response.data.errorMessages;
        if (errors.length) {
            toast.error(errors.join(`,`).replace(':', ''));
        }
    }, []);

    const resendCode = useCallback(async params => {
        await Api.post(`${baseUrl}/Account/ResendVerificationCode`, null, {
            params
        });
        return true;
    }, []);

    const requestPasswordReset = useCallback(async data => {
        const response = await Api.post<SignUpFlowResponse>(
            `${baseUrl}/Account/RequestPasswordReset`,
            qs.stringify(data)
        );
        return processSignUpData(response.data);
    }, []);

    const passwordReset = useCallback(async formData => {
        const response = await Api.post<SignUpFlowResponse>(
            `${baseUrl}/Account/PasswordReset`,
            qs.stringify(formData)
        );
        return processSignUpData(response.data);
    }, []);

    const logout = useCallback(async () => {
        await Api.get(`${baseUrl}/Account/Logout`);
        setState({ isAutentificated: false });
        setSession(null);
    }, []);

    const resetAuthState = useCallback(() => {
        setState(initialState);
    }, []);

    return {
        login,
        verifyPhone,
        resendCode,
        register,
        requestPasswordReset,
        passwordReset,
        logout,
        resetAuthState,
        setIsAutentificated
    };
}

export default AuthProvider;
export { useAuthState, useAuthDispatch };
