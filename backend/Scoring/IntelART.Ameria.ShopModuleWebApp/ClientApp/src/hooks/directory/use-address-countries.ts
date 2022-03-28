import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getAddressCountries } from 'queryies';

export const queryKey = `ADDRESS_COUNTRIES`;

function useAddressCountries(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getAddressCountries,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useAddressCountries;
