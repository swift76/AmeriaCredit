import { useQuery, QueryConfig } from 'react-query';
import { getUsername } from 'queryies';

export const queryKey = 'USERNAME';

type DataType = {
    userName: string;
    returnUrl: string;
};

export default function useUsername(config?: QueryConfig<DataType, any>) {
    return useQuery<DataType>({
        queryKey,
        queryFn: getUsername,
        config: {
            ...config
        }
    });
}
