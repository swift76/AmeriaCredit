import { IApplications } from 'types';
import { QueryConfig, useQuery } from 'react-query';
import { getApplications } from 'queryies';

export const queryKey = `APPLICATIONS`;

function useApplicationStatusText(id?: string | null, config?: QueryConfig<IApplications[], any>) {
    const { data: applications, isLoading } = useQuery<IApplications[], any>({
        queryKey,
        queryFn: getApplications,
        config
    });

    const application = applications?.find((app: IApplications) => app.ID === id);

    return {
        statusAm: application?.STATUS_AM,
        statusState: application?.STATUS_STATE,
        statusID: application?.STATUS_ID,
        isLoading
    };
}

export default useApplicationStatusText;
