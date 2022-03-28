import { IProfileData } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getRedirectToOnboardingSettings } from 'queryies';

export const queryKey = 'SETTINGS_REDIRECT_TO_ONBOARDING';

export default function useOnBoardingSettings(config?: QueryConfig<any, any>) {
    return useQuery<any>({
        queryKey,
        queryFn: getRedirectToOnboardingSettings,
        config
    });
}
