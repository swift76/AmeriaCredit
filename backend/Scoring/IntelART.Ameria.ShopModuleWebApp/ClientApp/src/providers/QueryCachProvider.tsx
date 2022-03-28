import React, { useEffect } from 'react';
import { ReactQueryDevtools } from 'react-query-devtools';
import { ReactQueryCacheProvider, QueryCache } from 'react-query';

type Props = {
    children: React.ReactNode;
};
const queryCache = new QueryCache({
    defaultConfig: {
        queries: {
            retry: 3,
            refetchOnWindowFocus: false
        }
    }
});

export default function QueryCachProvider({ children }: Props) {
    useEffect(() => {
        return () => {
            queryCache.clear();
        };
    }, []);

    return (
        <ReactQueryCacheProvider queryCache={queryCache}>
            {children}
            {process.env.NODE_ENV === 'development' && <ReactQueryDevtools />}
        </ReactQueryCacheProvider>
    );
}
