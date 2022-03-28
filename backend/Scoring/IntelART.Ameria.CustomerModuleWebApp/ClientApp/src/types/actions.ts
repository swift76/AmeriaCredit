export interface IFetchingAction {
    type: 'SET_FETCHING';
    fetching: boolean;
}

export interface ISubmittingAction {
    type: 'SET_SUBMITTING';
    submitting: boolean;
}
