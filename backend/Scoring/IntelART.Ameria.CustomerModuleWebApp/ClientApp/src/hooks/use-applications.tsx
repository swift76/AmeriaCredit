import { IApplications } from 'types';
import { QueryConfig, useQuery } from 'react-query';
import { getApplications } from 'queryies';

export const queryKey = `APPLICATIONS`;

function useApplications(config?: QueryConfig<IApplications[], any>) {
    return useQuery<IApplications[], any>({
        queryKey,
        queryFn: getApplications,
        config
    });
}

export default useApplications;
