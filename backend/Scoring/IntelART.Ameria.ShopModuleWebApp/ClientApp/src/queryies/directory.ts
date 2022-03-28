import API from 'services/api';

const url = `Directory`;

export async function getCountries() {
    const { data } = await API.get(`${url}/Countries`);
    return data;
}

export async function getAddressCountries() {
    const { data } = await API.get(`${url}/AddressCountries`);
    return data;
}

export async function getStates() {
    const { data } = await API.get(`${url}/States`);
    return data;
}

export async function getCitiesByStateCode(_: any, stateCode?: string) {
    if (!stateCode) {
        return [];
    }
    const { data } = await API.get(`${url}/States/${stateCode}/Cities`);
    return data;
}

export async function getCurrenciesByLoanType(_: any, type?: string) {
    if (!type) {
        return [];
    }
    const { data } = await API.get(`${url}/Currencies/${type}`);
    return data;
}

export async function getCommunicationTypes() {
    const { data } = await API.get(`${url}/CommunicationTypes`);
    return data;
}

export async function getIdDocumentTypes() {
    const { data } = await API.get(`${url}/IdDocumentTypes`);
    return data;
}

export async function getProductCategories() {
    const { data } = await API.get(`${url}/ProductCategories`);
    return data;
}
