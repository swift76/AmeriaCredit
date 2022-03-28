import API from 'services/api';
import { IApplications, IApplication, IApplicationMain, IApplicationAgreed, ICard } from 'types';

const path = `Applications`;

export async function getApplications() {
    const { data } = await API.get<IApplications[]>(path);
    return data;
}

export async function getApplication(_: any, id?: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}`);
    return data;
}

export async function getApplicationAgreed(_: any, id?: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}/Agreed`);
    return data;
}

export async function getApplicationMain(_: any, id?: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}/Main`);
    return data;
}

export async function saveApplication(values: IApplication) {
    return API.post(path, values);
}

export async function saveApplicationMain(values: IApplicationMain) {
    const { ID, ...rest } = values;
    return API.post(`${path}/${ID}/Main`, rest);
}

export async function saveApplicationAgree(values: IApplicationAgreed, id: string) {
    return API.post(`${path}/${id}/Agreed`, values);
}

export async function deleteApplication(id: string) {
    return API.delete(`${path}/${id}`);
}

export async function cancelApplication(id: string) {
    return API.put(`${path}/Cancelled/${id}`);
}

export async function printApplication(id: string) {
    const { data } = await API.get(`Applications/${id}/Documents/DOC_LOAN_CONTRACT_FINAL`);
    return data;
}

export async function getGeneralScoringResults(_: any, id: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}/GeneralScoringResults`);
    return data;
}

export async function getUploadedDocuments(_: any, id: string) {
    const { data } = await API.get(`${path}/${id}/Documents`);
    return data;
}

export async function getActiveCards(_: any, id: string) {
    const { data } = await API.get(`${path}/${id}/ActiveCards`);
    return data;
}

export async function getCreditCardTypes(_: any, id: string) {
    const { data } = await API.get(`${path}/${id}/CreditCardTypes`);
    return data;
}

export async function validateCard(values: ICard) {
    const { ID, ...rest } = values;
    return API.post(`${path}/${ID}/ValidateCard`, rest);
}

export async function checkCreditCardAuthorization(ID: string, code: string) {
    return API.post(`${path}/${ID}/CheckCreditCardAuthorization`, '"' + code + '"');
}

export async function creditCardAuthorization(ID: string) {
    return API.post(`${path}/${ID}/creditCardAuthorization`);
}
