import {
    UsePaginationInstanceProps,
    UsePaginationOptions,
    UsePaginationState,
    UseSortByInstanceProps,
    UseSortByOptions,
    UseSortByState,
    UseSortByColumnProps,
    UseFiltersOptions,
    UseFiltersInstanceProps,
    UseFiltersState,
    UseGlobalFiltersOptions,
    UseGlobalFiltersInstanceProps,
    UseGlobalFiltersState
} from 'react-table';

declare module 'react-table' {
    export interface TableOptions<D extends object = {}>
        extends UsePaginationOptions<D>,
            UseSortByOptions<D>,
            UseFiltersOptions<D>,
            UseGlobalFiltersOptions<D> {}

    export interface TableInstance<D extends object = {}>
        extends UsePaginationInstanceProps<D>,
            UseSortByInstanceProps<D>,
            UseFiltersInstanceProps<D>,
            UseGlobalFiltersInstanceProps<D> {}

    export interface TableState<D extends object = {}>
        extends UsePaginationState<D>,
            UseSortByState<D>,
            UseFiltersState<D>,
            UseGlobalFiltersState<D> {}

    export interface ColumnInstance<D extends object = {}> extends UseSortByColumnProps<D> {}
}
