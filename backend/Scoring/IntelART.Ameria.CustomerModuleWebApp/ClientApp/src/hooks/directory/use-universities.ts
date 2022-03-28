import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getUniversities } from 'queryies';

export const queryKey = `UNIVERSITIES`;

function useUniversities(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getUniversities,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useUniversities;
