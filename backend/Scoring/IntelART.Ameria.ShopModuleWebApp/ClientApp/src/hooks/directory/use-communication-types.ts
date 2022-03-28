import { useQuery, QueryConfig } from 'react-query';
import { DirectoryEntity } from 'types';
import { getCommunicationTypes } from 'queryies';

export const queryKey = `CommunicationTypes`;

function useCommunicationTypes(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getCommunicationTypes,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useCommunicationTypes;
