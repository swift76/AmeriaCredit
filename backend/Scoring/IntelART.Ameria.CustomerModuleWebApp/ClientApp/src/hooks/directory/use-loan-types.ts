import { ILoanTypes } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getLoanTypes } from 'queryies';

export const queryKey = `LOANTYPES`;

function useLoanTypes(isStudent: boolean, config?: QueryConfig<ILoanTypes[], any>) {
    return useQuery<ILoanTypes[]>({
        queryKey: [queryKey, isStudent],
        queryFn: getLoanTypes,
        config
    });
}

export default useLoanTypes;
