import { useEffect, useState } from 'react';
import { QueryConfig, useQuery } from 'react-query';
import { DirectoryEntity } from 'types';
import { getCitiesByStateCode } from 'queryies';

export const queryKey = `CITIES`;

function useCities(stateCode?: string | null, config?: QueryConfig<DirectoryEntity[], any>) {
    return useQuery<DirectoryEntity[]>({
        queryKey: [queryKey, stateCode],
        queryFn: getCitiesByStateCode,
        config
    });
}

export default useCities;
