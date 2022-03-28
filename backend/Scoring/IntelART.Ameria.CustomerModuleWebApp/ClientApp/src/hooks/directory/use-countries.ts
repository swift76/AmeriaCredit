import { DirectoryEntity } from 'types';
import { useQuery, QueryConfig } from 'react-query';
import { getCountries } from 'queryies';

export const queryKey = `COUNTRIES`;

function useCountries(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getCountries,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useCountries;
