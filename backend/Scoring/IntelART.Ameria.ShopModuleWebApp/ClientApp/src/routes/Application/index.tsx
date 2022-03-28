import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { TransitionGroup, CSSTransition } from 'react-transition-group';
import { useApplication } from 'hooks';
import { APPLICATION_STATUS_ENUM as statuses, TabsKeyEnum } from 'constants/application';
import TabContainer from 'containers/TabContainer';
import ApplicationProvider, { useApplicationState, useApplicationDispatch } from './Provider';
import ApplicationTabs from './components/ApplicationTabs';
import useTabAccess from './hooks/use-tabs-access';
import PreApplication from './tabs/PreApplication';
import ApplicationMain from './tabs/ApplicationMain';

const { PRE_APPLICATION, APPLICATION_MAIN } = TabsKeyEnum;

function Application() {
    const { tabName } = useApplicationState();
    const { setActiveTabName } = useApplicationDispatch();
    const { id: applicationId = null } = useParams();
    const { applicationId: applicationIdContext } = useApplicationState();
    const { setApplicationId } = useApplicationDispatch();

    const { data: applicationData } = useApplication(applicationId, { enabled: !!applicationId });
    const status = applicationData?.STATUS_STATE || null;

    const { isTabDisabled } = useTabAccess(status, tabName);

    useEffect(() => {
        if (!applicationIdContext && applicationId) {
            setApplicationId(applicationId);
        }
    }, [applicationIdContext, applicationId, setApplicationId]);

    useEffect(() => {
        if (
            status &&
            [
                statuses.PRE_APPROVAL_SUCCESS,
                statuses.AGREED,
                statuses.CANCELLED,
                statuses.DELIVERING,
                statuses.COMPLETED
            ].includes(status)
        ) {
            setActiveTabName(TabsKeyEnum.APPLICATION_MAIN);
        }
    }, [setActiveTabName, status]);

    return (
        <>
            <ApplicationTabs />
            <TransitionGroup>
                <CSSTransition key={tabName} timeout={200} classNames="tab">
                    <TabContainer>
                        {tabName === PRE_APPLICATION && <PreApplication disabled={isTabDisabled} />}
                        {tabName === APPLICATION_MAIN && (
                            <ApplicationMain disabled={isTabDisabled} />
                        )}
                    </TabContainer>
                </CSSTransition>
            </TransitionGroup>
        </>
    );
}

export default () => (
    <ApplicationProvider>
        <Application />
    </ApplicationProvider>
);
