import { useQuery, QueryConfig } from 'react-query';
import { getUserRole } from 'queryies';

export const queryKey = 'USER_ROLE';

type DataType = number;

export default function useUserRole(config?: QueryConfig<DataType, any>) {
    return useQuery<DataType>({
        queryKey,
        queryFn: getUserRole,
        config: {
            ...config
        }
    });
}
