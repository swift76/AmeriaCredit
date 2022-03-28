import { useEffect, useState } from 'react';
import { QueryConfig, useQuery } from 'react-query';
import { DirectoryEntity } from 'types';
import { getCurrenciesByLoanType } from 'queryies';

export const queryKey = `CURRENCIES`;

function useCurrencies(stateCode?: string | null, config?: QueryConfig<DirectoryEntity[], any>) {
    const [enabled, setEnabled] = useState(!!stateCode);

    useEffect(() => {
        if (stateCode) {
            setEnabled(!!stateCode);
        }
    }, [stateCode]);

    return useQuery<DirectoryEntity[]>({
        queryKey: [queryKey, stateCode],
        queryFn: getCurrenciesByLoanType,
        config: {
            ...config,
            staleTime: Infinity,
            enabled
        }
    });
}

export default useCurrencies;
