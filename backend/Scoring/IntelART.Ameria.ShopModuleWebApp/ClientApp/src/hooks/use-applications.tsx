import { IApplications } from 'types';
import { QueryConfig, useQuery } from 'react-query';
import { getApplications } from 'queryies';

export const queryKey = `APPLICATIONS`;

function useApplications(
    from?: string | null,
    to?: string | null,
    config?: QueryConfig<IApplications[], any>
) {
    return useQuery<IApplications[], any>({
        queryKey: from && to && [queryKey, from, to],
        queryFn: getApplications,
        config: {
            ...config
        }
    });
}

export default useApplications;
