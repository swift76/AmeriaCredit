import { useQuery, QueryConfig } from 'react-query';
import { DirectoryEntity } from 'types';
import { getProductCategories } from 'queryies';

export const queryKey = `ProductCategories`;

function useProductCategories(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getProductCategories,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useProductCategories;
