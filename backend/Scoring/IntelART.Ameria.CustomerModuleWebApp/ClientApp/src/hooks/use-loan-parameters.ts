import { QueryConfig, useQuery } from 'react-query';
import { ILoanParameters } from 'types';
import { getLoanParameters } from 'queryies';

export const queryKey = `LOANPARAMETERS`;

export default function useLoanParameters(
    loanTypeCode?: string | null,
    config?: QueryConfig<ILoanParameters, any>
) {
    return useQuery<ILoanParameters>({
        queryKey: loanTypeCode && [queryKey, loanTypeCode],
        queryFn: getLoanParameters,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}
