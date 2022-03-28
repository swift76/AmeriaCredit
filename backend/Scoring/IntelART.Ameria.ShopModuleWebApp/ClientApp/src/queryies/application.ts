import API from 'services/api';
import { IApplications, IPreApplication, IApplicationMain } from 'types';
import { BASE_URL } from '../constants/api';

const path = `Applications`;

export async function getApplications(_: any, from?: string, to?: string) {
    const { data } = await API.get<IApplications[]>(`${path}/?fromDate=${from}&toDate=${to}`);
    return data;
}

export async function getApplication(_: any, id?: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}`);
    return data;
}

export async function getApplicationMain(_: any, id?: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}/Main`);
    return data;
}

export async function saveApplication(values: IPreApplication) {
    return API.post(path, values);
}

export async function saveApplicationMain(values: IApplicationMain) {
    const { ID, ...rest } = values;
    return API.post(`${path}/${ID}/Main`, rest);
}

export async function deleteApplication(id: string) {
    return API.delete(`${path}/${id}`);
}

export async function rejectApplication(id: string) {
    return API.put(`${path}/Rejected/${id}`);
}

export async function printApplication(id: string) {
    const { data } = await API.get(`Applications/${id}/Documents/DOC_LOAN_CONTRACT_FINAL`, {
        responseType: 'blob'
    });
    return data;
}

export async function getInstallationScoringResult(_: any, id: string) {
    if (!id) {
        return undefined;
    }
    const { data } = await API.get(`${path}/${id}/InstallationScoringResult`);
    return data;
}

export async function getUploadedDocuments(_: any, id: string) {
    const { data } = await API.get(`${path}/${id}/Documents`);
    return data;
}

export async function getTemplateResults(_: any, product: string) {
    const { data } = await API.get(
        `${path}/InstallationTemplateResults?productCategoryCode=${product}`
    );
    return data;
}

export async function mobilePhoneAuthorization(applicationID: string) {
    return API.post(`${path}/MobilePhoneAuthorization/${applicationID}`);
}
