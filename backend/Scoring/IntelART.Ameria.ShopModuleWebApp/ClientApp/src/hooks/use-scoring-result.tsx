import { QueryConfig, useQuery } from 'react-query';
import { IScoringResults } from 'types';
import { getInstallationScoringResult } from 'queryies';

export const queryKey = `SCORING`;

export default function useScoringResult(
    id?: string | null,
    config?: QueryConfig<IScoringResults, any>
) {
    return useQuery<IScoringResults, any>({
        queryKey: id && [queryKey, id],
        queryFn: getInstallationScoringResult,
        config: {
            ...config
        }
    });
}
