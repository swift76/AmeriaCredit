import API from 'services/api';

export async function getLoanLimits(_: any, loanTypeCode?: string, currency?: string) {
    if (!loanTypeCode && !currency) {
        return undefined;
    }
    const { data } = await API.get(`LoanLimits`, { params: { loanTypeCode, currency } });
    return data;
}

export async function getRepaymentSchedule(params: any) {
    const { interest, amount, duration, serviceInterest = 0, serviceAmount = 0 } = params;
    const { data } = await API.get(`RepaymentSchedule`, {
        params: {
            interest,
            amount,
            duration,
            serviceInterest,
            serviceAmount
        }
    });

    return data;
}
