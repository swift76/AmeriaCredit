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

type State = {
    isAutentificated: boolean;
    processID?: string;
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

    const login = useCallback(async ({ Username, Password }) => {
        try {
            const response = await Api.post<SignInResponse>(
                `${baseUrl}/Account/Login`,
                qs.stringify({ Username, Password })
            );
            setSession(response.data.access_token);
            setState({ isAutentificated: true });
        } catch (error) {
            if (error && error.error_description) {
                toast.error(error.error_description);
            } else {
                toast.error(`Առկա է տեխնիկական խնդիր`);
            }
            throw error;
        }
    }, []);

    const passwordReset = useCallback(async formdata => {
        await Api.post(`${baseUrl}/Account/ChangePassword`, qs.stringify(formdata));
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
        passwordReset,
        logout,
        resetAuthState
    };
}

export default AuthProvider;
export { useAuthState, useAuthDispatch };
