import { QueryConfig, useQuery } from 'react-query';
import { DirectoryEntity } from 'types';
import { getCreditCardTypes } from 'queryies';

export const queryKey = `CREDITCARDTYPES`;

export default function useCreditCardTypes(
    id?: string | null,
    config?: QueryConfig<DirectoryEntity[], any>
) {
    return useQuery<DirectoryEntity[], any>({
        queryKey: id && [queryKey, id],
        queryFn: getCreditCardTypes,
        config
    });
}
