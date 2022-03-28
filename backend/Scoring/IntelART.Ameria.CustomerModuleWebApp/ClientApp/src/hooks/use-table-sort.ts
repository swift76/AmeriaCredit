/* eslint-disable react-hooks/exhaustive-deps */
import { useState, useCallback, useMemo } from 'react';
import { desc, stableSort } from 'helpers/table';

type Order = 'asc' | 'desc';

const useTableSort = <C, I extends keyof C>(dataSource: C[], initialState: I) => {
    const [order, setOrder] = useState<Order>('asc');
    const [orderBy, setOrderBy] = useState<keyof C>(initialState);

    const sortRequestHandler = useCallback(
        (property: keyof C) => {
            const isDesc = orderBy === property && order === 'desc';
            setOrder(isDesc ? 'asc' : 'desc');
            setOrderBy(property);
        },
        [order, orderBy]
    );

    const getSortType = useCallback(<K extends keyof C>(sortOrder: Order, sortOrderBy: K): ((
        a: C,
        b: C
    ) => number) => {
        return sortOrder === 'desc'
            ? (a, b) => desc(a, b, sortOrderBy)
            : (a, b) => -desc(a, b, sortOrderBy);
    }, []);

    const createSortHandler = useCallback(
        (property: keyof C) => {
            return () => {
                sortRequestHandler(property);
            };
        },
        [sortRequestHandler]
    );
    const sortedRows = useMemo(() => stableSort(dataSource, getSortType(order, orderBy)), [
        dataSource,
        order,
        orderBy,
        getSortType
    ]);
    return {
        sortedRows,
        createSortHandler,
        order,
        orderBy
    };
};
export default useTableSort;
