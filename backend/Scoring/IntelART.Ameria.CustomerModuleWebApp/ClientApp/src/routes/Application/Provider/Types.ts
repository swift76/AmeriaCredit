import { TabsKeysEnumTypes } from 'types';

export const Types = {
    SET_ACTIVE_TAB: 'SET_ACTIVE_TAB',
    SET_MODAL_STATE: 'SET_MODAL_STATE'
} as const;

interface SetActiveTabAction {
    type: typeof Types['SET_ACTIVE_TAB'];
    tabName: TabsKeysEnumTypes;
}

interface SetModalStateAction {
    type: typeof Types['SET_MODAL_STATE'];
    state: boolean;
}

export type Action = SetActiveTabAction | SetModalStateAction;

export type State = {
    tabName: TabsKeysEnumTypes;
    isModalShow: boolean;
};
export type Dispatch = (action: Action) => void;
