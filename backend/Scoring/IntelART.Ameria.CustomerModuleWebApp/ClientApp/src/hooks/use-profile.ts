import { IProfileData } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getProfile } from 'queryies';

export const queryKey = 'PROFILE';

export default function useProfile(config?: QueryConfig<IProfileData[], any>) {
    return useQuery<IProfileData>({
        queryKey,
        queryFn: getProfile,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}
