import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getBankBranches } from 'queryies';

export const queryKey = 'BANK_BRANCHES';

function useBankBranches(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getBankBranches,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useBankBranches;
