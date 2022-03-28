import { QueryConfig, useQuery } from 'react-query';
import { ITemplateResults } from 'types';
import { getTemplateResults } from 'queryies';

export const queryKey = `TEMPLATE_RESULT`;

export default function useTemplateResult(
    product?: string | null,
    config?: QueryConfig<ITemplateResults[], any>
) {
    return useQuery<ITemplateResults[], any>({
        queryKey: product && [queryKey, product],
        queryFn: getTemplateResults,
        config: {
            ...config
        }
    });
}
