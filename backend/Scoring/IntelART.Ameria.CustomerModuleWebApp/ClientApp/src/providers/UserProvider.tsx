import React, { useEffect, createContext, useContext, useState } from 'react';
import { IProfileData } from 'types';
import { useProfile, useOnBoardFrom } from 'hooks';
import { getUserInfoFromOnBoard } from 'helpers/data';

type UserProviderProps = { children: React.ReactNode };

const UserStateContext = createContext<any | undefined>(undefined);

function UserProvider({ children }: UserProviderProps) {
    const [state, setState] = useState<IProfileData | null>(null);
    const { data: profileData } = useProfile();
    const onboardId = profileData?.ONBOARDING_ID;
    const { data: onBoardFromData } = useOnBoardFrom(onboardId, {
        enabled: !!onboardId
    });

    // if user from onboarding get onboarding data and merger with profileData
    useEffect(() => {
        let data = null;
        if (profileData) {
            data = { ...profileData };
        }

        if (onBoardFromData) {
            const dataFromOnBoard = getUserInfoFromOnBoard(onBoardFromData);
            Object.assign(data, dataFromOnBoard);
        }

        setState(data);
    }, [profileData, onBoardFromData]);

    return <UserStateContext.Provider value={state}>{children}</UserStateContext.Provider>;
}

function useUserState() {
    const state = useContext(UserStateContext);

    if (state === undefined) {
        throw new Error('useUserState must be used within a UserProvider');
    }

    return state;
}

export default UserProvider;

export { useUserState };
