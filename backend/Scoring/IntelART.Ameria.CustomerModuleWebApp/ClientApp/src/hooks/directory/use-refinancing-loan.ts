import { useQuery, QueryConfig } from 'react-query';
import { ILoanRefinancing } from 'types';
import { getRefinancingLoan } from 'queryies';

export const queryKey = `getRefinancingLoan`;

function useRefinancingLoan(applicationId: string, config?: QueryConfig<any>) {
    return useQuery<ILoanRefinancing[]>({
        queryKey: applicationId,
        queryFn: getRefinancingLoan,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useRefinancingLoan;
