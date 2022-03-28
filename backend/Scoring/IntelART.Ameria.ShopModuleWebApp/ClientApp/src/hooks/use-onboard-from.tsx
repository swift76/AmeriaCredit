import { QueryConfig, useQuery } from 'react-query';
import { IOnboardingData } from 'types';
import { getOnBoardingDataFrom } from 'queryies';

export const queryKey = `ONBOARD_FROM`;

export default function useOnBoardFrom(
    id?: string | null,
    config?: QueryConfig<IOnboardingData, any>
) {
    return useQuery<IOnboardingData, any>({
        queryKey: id && [queryKey, id],
        queryFn: getOnBoardingDataFrom,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}
