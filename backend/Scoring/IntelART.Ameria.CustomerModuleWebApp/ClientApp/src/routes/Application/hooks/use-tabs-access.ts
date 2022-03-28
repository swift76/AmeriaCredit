import { useState, useEffect } from 'react';
import { getTabAccessByStatus } from 'helpers/data';
import { TabsKeyEnum } from 'constants/application';
import { TabsKeysEnumTypes } from 'types';

const initialState = {
    isTabEnabled: false,
    isTabAccessible: false
};

export default function useTabsAccess(status: string | null, name: TabsKeysEnumTypes) {
    const [state, setState] = useState(initialState);

    useEffect(() => {
        if (status && name) {
            setState(getTabAccessByStatus(status, name));
        } else if (name === TabsKeyEnum.BASE) {
            setState({ isTabEnabled: true, isTabAccessible: true });
        } else {
            setState(initialState);
        }
    }, [status, name]);

    return state;
}
