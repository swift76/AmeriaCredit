import { BASE_URL } from 'constants/api';
import API from 'services/api';
import { IOnboardingData } from 'types';

export async function getOnBoardingDataFrom(
    _: any,
    id: string | null
): Promise<IOnboardingData | undefined> {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${BASE_URL}/DataFrom/${id}`);
    return data;
}

export async function getOnBoardingDataTo(id: string | null) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${BASE_URL}DataTo/${id}`);
    return data;
}

export async function getOnBoardRedirectionLink(id: string | null) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${BASE_URL}/RedirectionLink/${id}`);
    return data;
}
