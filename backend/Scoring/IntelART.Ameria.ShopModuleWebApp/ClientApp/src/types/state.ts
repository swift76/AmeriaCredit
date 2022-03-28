export type IHookState<D> = {
    data: undefined | D;
    fetching: boolean;
    fetched: boolean;
    submitting: boolean;
};
