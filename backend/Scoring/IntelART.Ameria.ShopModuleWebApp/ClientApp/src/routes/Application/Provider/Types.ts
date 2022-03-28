import { TabsKeysEnumTypes } from 'types';

export const Types = {
    SET_ACTIVE_TAB: 'SET_ACTIVE_TAB',
    SET_APPLICATION_ID: 'SET_APPLICATION_ID'
} as const;

interface SetActiveTabAction {
    type: typeof Types['SET_ACTIVE_TAB'];
    tabName: TabsKeysEnumTypes;
}

interface SetApplicationId {
    type: typeof Types['SET_APPLICATION_ID'];
    applicationId: string;
}

export type Action = SetActiveTabAction | SetApplicationId;

export type State = {
    tabName: TabsKeysEnumTypes;
    applicationId: string;
};
export type Dispatch = (action: Action) => void;
