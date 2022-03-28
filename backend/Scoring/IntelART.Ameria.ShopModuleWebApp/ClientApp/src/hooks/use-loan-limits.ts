import { QueryConfig, useQuery } from 'react-query';
import { ILoanLimits } from 'types';
import { getLoanLimits } from 'queryies';

export const queryKey = `LOANLIMITS`;

export default function useLoanLimits(
    loanTypeCode?: string | null,
    currency?: string | null,
    config?: QueryConfig<ILoanLimits, any>
) {
    return useQuery<ILoanLimits>({
        queryKey: loanTypeCode && currency && [queryKey, loanTypeCode, currency],
        queryFn: getLoanLimits,
        config: {
            ...config,
            staleTime: Infinity,
            enabled: !!loanTypeCode && !!currency
        }
    });
}
