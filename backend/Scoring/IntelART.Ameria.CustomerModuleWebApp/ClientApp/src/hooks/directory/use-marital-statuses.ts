import { useQuery, QueryConfig } from 'react-query';
import { DirectoryEntity } from 'types';
import { getMaritalStatuses } from 'queryies';

export const queryKey = `MaritalStatuses`;

function useMaritalStatuses(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getMaritalStatuses,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useMaritalStatuses;
