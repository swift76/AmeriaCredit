import { useQuery, QueryConfig } from 'react-query';
import { DirectoryEntity } from 'types';
import { getMonthlyNetIncomeRanges } from 'queryies';

export const queryKey = `MonthlyNetIncomeRanges`;

function useMonthlyNetIncomeRanges(config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey,
        queryFn: getMonthlyNetIncomeRanges,
        config: {
            ...config,
            staleTime: Infinity
        }
    });
}

export default useMonthlyNetIncomeRanges;
