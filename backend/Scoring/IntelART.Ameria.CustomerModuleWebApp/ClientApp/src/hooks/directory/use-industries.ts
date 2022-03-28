import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getIndustries } from 'queryies';

export const queryKey = `Industries`;

function useIndustries(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getIndustries,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useIndustries;
