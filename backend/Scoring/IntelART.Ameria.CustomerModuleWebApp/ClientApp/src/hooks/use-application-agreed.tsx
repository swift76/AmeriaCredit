import { QueryConfig, useQuery } from 'react-query';
import { IApplicationAgreed } from 'types';
import { getApplicationAgreed } from 'queryies';

export const queryKey = `APPLICATION_AGREED`;

export default function useApplicationAgreed(
    id?: string | null,
    config?: QueryConfig<IApplicationAgreed, any>
) {
    return useQuery<IApplicationAgreed, any>({
        queryKey: id && [queryKey, id],
        queryFn: getApplicationAgreed,
        config
    });
}
