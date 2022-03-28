import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useApplication } from 'hooks';
import { TabsKeyEnum } from 'constants/application';
import PageContainer from 'containers/PageContainer';
import ApplicationProvider, { useApplicationState } from './Provider';
import ApplicationTabs from './components/ApplicationTabs';
import useTabAccess from './hooks/use-tabs-access';

import ApplicationBase from './pages/ApplicationBase';
import ApplicationDetails from './pages/ApplicationDetails';
import ApplicationContract from './pages/ApplicationContract';
import ApplicationVerification from './pages/ApplicationVerification';

const { BASE, DETAILS, CONTRACT, VERIFICATION } = TabsKeyEnum;

function Application() {
    const { tabName } = useApplicationState();
    const { id: applicationId = null } = useParams();

    const { data: applicationData } = useApplication(applicationId, { enabled: !!applicationId });
    const status = applicationData?.STATUS_STATE || null;
    const { isTabEnabled, isTabAccessible } = useTabAccess(status, tabName);

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [tabName]);

    return (
        <>
            <ApplicationTabs status={status} />

            {tabName === BASE && <ApplicationBase disabled={!isTabEnabled} />}

            {applicationId && isTabAccessible && (
                <>
                    {tabName === DETAILS && (
                        <ApplicationDetails id={applicationId} disabled={!isTabEnabled} />
                    )}

                    {tabName === VERIFICATION && (
                        <ApplicationVerification id={applicationId} disabled={!isTabEnabled} />
                    )}

                    {tabName === CONTRACT && (
                        <ApplicationContract id={applicationId} disabled={!isTabEnabled} />
                    )}
                </>
            )}
        </>
    );
}

export default () => (
    <ApplicationProvider>
        <PageContainer>
            <Application />
        </PageContainer>
    </ApplicationProvider>
);
