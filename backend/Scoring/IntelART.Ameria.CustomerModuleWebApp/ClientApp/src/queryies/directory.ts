import API from 'services/api';

const url = `Directory`;

export async function getCountries() {
    const { data } = await API.get(`${url}/Countries`);
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

export async function getBankBranches() {
    const { data } = await API.get(`${url}/BankBranches`);
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

export async function getIndustries() {
    const { data } = await API.get(`${url}/Industries`);
    return data;
}

export async function getLoanTypes(_: any, isStudent: boolean) {
    const { data } = await API.get(`${url}/LoanTypes/${isStudent}`);
    return data;
}

export async function getMaritalStatuses() {
    const { data } = await API.get(`${url}/MaritalStatuses`);
    return data;
}

export async function getMonthlyNetIncomeRanges() {
    const { data } = await API.get(`${url}/MonthlyNetIncomeRanges`);
    return data;
}

export async function getProductCategories() {
    const { data } = await API.get(`${url}/ProductCategories`);
    return data;
}

export async function getProductReceivingOptions() {
    const { data } = await API.get(`${url}/ProductReceivingOptions`);
    return data;
}

export async function getWorkExperienceDurations() {
    const { data } = await API.get(`${url}/WorkExperienceDurations`);
    return data;
}

export async function getUniversities() {
    const { data } = await API.get(`${url}/Universities`);
    return data;
}
