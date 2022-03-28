import { QueryConfig, useQuery } from 'react-query';
import { IActiveCards } from 'types';
import { getActiveCards } from 'queryies';

export const queryKey = `ACTIVECARDS`;

export default function useActiveCards(
    id?: string | null,
    config?: QueryConfig<IActiveCards[], any>
) {
    return useQuery<IActiveCards[], any>({
        queryKey: id && [queryKey, id],
        queryFn: getActiveCards,
        config
    });
}
