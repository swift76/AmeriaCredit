import { useQuery, QueryConfig } from 'react-query';
import { getFileMaxSize } from 'queryies';

export const queryKey = 'FILE_MAX_SIZE';

function useFileMaxSize(config?: QueryConfig<number, any>) {
    return useQuery<number>({
        queryKey,
        queryFn: getFileMaxSize,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useFileMaxSize;
