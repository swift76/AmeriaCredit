import { QueryConfig, useQuery } from 'react-query';
import { IApplicationMain } from 'types';
import { getApplicationMain } from 'queryies';

export const queryKey = `APPLICATION_MAIN`;

export default function useApplicationMain(
    id?: string | null,
    config?: QueryConfig<IApplicationMain, any>
) {
    return useQuery<IApplicationMain, any>({
        queryKey: id && [queryKey, id],
        queryFn: getApplicationMain,
        config: {
            ...config
        }
    });
}
