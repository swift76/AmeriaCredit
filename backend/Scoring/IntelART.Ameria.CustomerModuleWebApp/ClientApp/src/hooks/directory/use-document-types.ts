import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getIdDocumentTypes } from 'queryies';

export const queryKey = `IdDocumentTypes`;

function useDocumentTypes(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getIdDocumentTypes,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useDocumentTypes;
