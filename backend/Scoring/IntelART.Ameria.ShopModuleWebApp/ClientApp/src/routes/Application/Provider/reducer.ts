import { Types, State, Action } from './Types';

const applicationReducer = (state: State, action: Action) => {
    switch (action.type) {
        case Types.SET_ACTIVE_TAB:
            return { ...state, tabName: action.tabName };
        case Types.SET_APPLICATION_ID:
            return { ...state, applicationId: action.applicationId };
        default:
            return state;
    }
};

export default applicationReducer;
