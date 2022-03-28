import { useQuery, QueryConfig } from 'react-query';
import { DirectoryEntity } from 'types';
import { getProductReceivingOptions } from 'queryies';

export const queryKey = `ProductReceivingOptions`;
function useProductReceiving(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getProductReceivingOptions,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useProductReceiving;
