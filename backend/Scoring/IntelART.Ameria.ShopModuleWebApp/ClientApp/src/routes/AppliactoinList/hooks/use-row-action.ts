import { useState, useEffect } from 'react';
import { APPLICATION_STATUS_ENUM as statusesEnum, ActionTypes } from 'constants/application';

export function useRowActionType(status: string) {
    const [actionType, setActionType] = useState<string | null>();

    useEffect(() => {
        switch (status) {
            case statusesEnum.NEW:
                setActionType(ActionTypes.REMOVE);
                break;
            case statusesEnum.PENDING_PRE_APPROVAL:
            case statusesEnum.PENDING_APPROVAL:
            case statusesEnum.PRE_APPROVAL_SUCCESS:
            case statusesEnum.APPROVAL_SUCCESS:
            case statusesEnum.AGREED:
                setActionType(ActionTypes.CANCEL);
                break;
            case statusesEnum.DELIVERING:
            case statusesEnum.COMPLETED:
                setActionType(ActionTypes.PRINT);
                break;
            case statusesEnum.APPROVAL_REVIEW:
                setActionType(ActionTypes.APPROACHBANK);
                break;

            default:
                setActionType(null);
                break;
        }
    }, [status]);

    return { actionType };
}
