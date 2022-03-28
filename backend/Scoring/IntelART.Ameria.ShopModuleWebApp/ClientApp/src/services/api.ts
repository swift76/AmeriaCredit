/* eslint-disable prefer-promise-reject-errors */
import axios, { AxiosError } from 'axios';
import { BASE_URL, API_URL } from 'constants/api';
import { decodeToken } from 'utils';

const instance = axios.create({
    withCredentials: true,
    baseURL: `${BASE_URL}/${API_URL}`
});

export const storedToken = localStorage.getItem('token');

export const isValidToken = (token: string | null) => {
    if (!token) return false;

    const decoded = decodeToken(token);
    const currentTime = Date.now() / 1000;
    if (decoded.exp < currentTime) {
        console.warn('access token expired');
        return false;
    }
    return true;
};

export const setSession = (token: string | null) => {
    if (token) {
        localStorage.setItem('token', token);
        instance.defaults.headers.common.Authorization = `Bearer ${token}`;
    } else {
        localStorage.removeItem('token');
        delete instance.defaults.headers.common.Authorization;
    }
};

if (storedToken) {
    if (isValidToken(storedToken)) {
        setSession(storedToken);
    } else {
        setSession(null);
    }
}

instance.interceptors.response.use(
    response => response,
    (error: AxiosError) => {
        const { response } = error;
        return new Promise((_, reject) => {
            if (response?.data) {
                if (
                    response.status === 401 ||
                    (response.data.Message &&
                        response.data.Message.search('No authenticationScheme was specified') >= 0)
                ) {
                    setSession(null);
                    window.location.pathname = '/signin';
                    console.error(response.data);
                }
                return reject({ ...response.data, status: response.status });
            }

            return reject(error);
        });
    }
);

export default instance;
