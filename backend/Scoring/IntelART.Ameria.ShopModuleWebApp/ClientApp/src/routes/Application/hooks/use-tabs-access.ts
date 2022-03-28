import { useState, useEffect } from 'react';
import { getTabAccessByStatus } from 'helpers/data';
import { TabsKeyEnum } from 'constants/application';
import { TabsKeysEnumTypes } from 'types';

const initialState = {
    isTabDisabled: true
};

export default function useTabsAccess(status: string | null, name: TabsKeysEnumTypes) {
    const [state, setState] = useState(initialState);

    useEffect(() => {
        if (status && name) {
            setState(getTabAccessByStatus(status, name));
        } else if (name === TabsKeyEnum.PRE_APPLICATION) {
            setState({ isTabDisabled: false });
        } else {
            setState(initialState);
        }
    }, [status, name]);

    return state;
}
