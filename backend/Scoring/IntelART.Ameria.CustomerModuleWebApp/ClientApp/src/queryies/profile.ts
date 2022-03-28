import API from 'services/api';
import { BASE_URL } from 'constants/api';
import { IProfileData } from 'types';

const profileUrl = `${BASE_URL}/api/customer/Profile`;

export async function saveProfile(values: IProfileData) {
    return API.post(profileUrl, values);
}

export async function getProfile() {
    const { data } = await API.get<IProfileData>(profileUrl);
    if (data) {
        data.MOBILE_PHONE = data.MOBILE_PHONE.replace(/\s/g, '');
        return data;
    }
    return undefined;
}
