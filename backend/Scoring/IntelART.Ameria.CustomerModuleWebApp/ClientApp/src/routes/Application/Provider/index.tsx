import React, { useCallback, createContext, useContext, useReducer, ReactNode } from 'react';
import { TabsKeysEnumTypes } from 'types';
import applicationReducer from './reducer';
import { initialState } from './initialState';
import { Types, State, Dispatch } from './Types';

type Props = { children: ReactNode };

const ApplicationStateContext = createContext<State | undefined>(undefined);
const ApplicationDispatchContext = createContext<Dispatch | undefined>(undefined);

function ApplicationProvider({ children }: Props) {
    const [state, dispatch] = useReducer(applicationReducer, initialState);

    return (
        <ApplicationStateContext.Provider value={state}>
            <ApplicationDispatchContext.Provider value={dispatch}>
                {children}
            </ApplicationDispatchContext.Provider>
        </ApplicationStateContext.Provider>
    );
}

function useApplicationState() {
    const context = useContext(ApplicationStateContext);
    if (context === undefined) {
        throw new Error('useApplicationState must be used within a ApplicationProvider');
    }
    return context;
}

function useApplicationDispatch() {
    const dispatch = useContext(ApplicationDispatchContext);

    if (dispatch === undefined) {
        throw new Error('useApplicationDispatch must be used within a ApplicationProvider');
    }

    const setActiveTabName = useCallback(
        (tabName: TabsKeysEnumTypes) => {
            dispatch({ type: Types.SET_ACTIVE_TAB, tabName });
        },
        [dispatch]
    );

    const setModalState = useCallback(
        (state: boolean) => {
            dispatch({ type: Types.SET_MODAL_STATE, state });
        },
        [dispatch]
    );

    return { setActiveTabName, setModalState };
}

export default ApplicationProvider;
export { useApplicationState, useApplicationDispatch };
