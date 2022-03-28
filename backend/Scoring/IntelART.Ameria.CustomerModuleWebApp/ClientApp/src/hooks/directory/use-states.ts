import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getStates } from 'queryies';

export const queryKey = 'STATES';

function useStates(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getStates,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useStates;
