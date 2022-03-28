import { QueryConfig, useQuery } from 'react-query';
import { IApplication } from 'types';
import { getApplication } from 'queryies';

export const queryKey = `APPLICATION`;

export default function useApplication(
    id?: string | null,
    config?: QueryConfig<IApplication, any>
) {
    return useQuery<IApplication, any>({
        queryKey: id && [queryKey, id],
        queryFn: getApplication,
        config: {
            ...config
        }
    });
}
