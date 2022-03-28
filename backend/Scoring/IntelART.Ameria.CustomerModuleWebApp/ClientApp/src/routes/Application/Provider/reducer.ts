import { Types, State, Action } from './Types';

const applicationReducer = (state: State, action: Action): State => {
    switch (action.type) {
        case Types.SET_ACTIVE_TAB:
            return { ...state, tabName: action.tabName };
        case Types.SET_MODAL_STATE:
            return { ...state, isModalShow: action.state };

        default:
            return state;
    }
};

export default applicationReducer;
