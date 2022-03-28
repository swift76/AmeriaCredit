import { useQuery, QueryConfig } from 'react-query';
import { DirectoryEntity } from 'types';
import { getWorkExperienceDurations } from 'queryies';

export const queryKey = `WorkExperienceDurations`;

function useWorkExperienceDurations(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getWorkExperienceDurations,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useWorkExperienceDurations;
