import API from 'services/api';

export async function getLoanLimits(_: any, loanTypeCode?: string, currency?: string) {
    if (!loanTypeCode && !currency) {
        return undefined;
    }
    const { data } = await API.get(`LoanLimits`, { params: { loanTypeCode, currency } });
    return data;
}

export async function getLoanParameters(_: any, loanTypeCode?: string) {
    if (!loanTypeCode) {
        return undefined;
    }
    const { data } = await API.get(`Parameters`, { params: { loanTypeCode } });
    return data;
}

export async function getRefinancingLoan(applicationId?: string) {
    const { data } = await API.get(`RefinancingLoan/${applicationId}`);
    return data;
}

export async function saveRefinancingLoan(values: any) {
    return API.post(`RefinancingLoan`, values);
}

export async function getRepaymentSchedule(params: any) {
    const { data } = await API.get(`RepaymentSchedule`, { params });

    return data;
}
