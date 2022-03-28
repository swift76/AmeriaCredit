import API from 'services/api';
import { BASE_URL, API_URL } from 'constants/api';

export async function getUsername() {
    const { data } = await API.get(`${BASE_URL}/account/ChangePassword`);

    return data;
}

export async function getUserRole() {
    const { data } = await API.get(`${BASE_URL}/${API_URL}/Applications/UserRole`);

    return data;
}
