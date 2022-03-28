import React, { useMemo, useEffect } from 'react';
import Nav from 'react-bootstrap/Nav';
import clsx from 'clsx';
import { TabsKeyEnum } from 'constants/application';
import { TabsKeysEnumTypes } from 'types';
import { useApplicationDispatch, useApplicationState } from '../Provider';
import useTabsAccess from '../hooks/use-tabs-access';

type Props = {
    status: string | null;
};

const { BASE, DETAILS, CONTRACT, VERIFICATION } = TabsKeyEnum;

function ApplicationTabs({ status }: Props) {
    const { tabName, isModalShow } = useApplicationState();
    const { setActiveTabName } = useApplicationDispatch();

    const handleNavClick = (eventKey: string | null) => {
        if (eventKey) {
            setActiveTabName(eventKey as TabsKeysEnumTypes);
        }
    };

    const detailsConfig = useTabsAccess(status, DETAILS);
    const verificationConfig = useTabsAccess(status, VERIFICATION);
    const contractConfig = useTabsAccess(status, CONTRACT);

    useEffect(() => {
        if (!isModalShow) {
            if (contractConfig.isTabAccessible) {
                setActiveTabName(CONTRACT);
            } else if (verificationConfig.isTabAccessible) {
                setActiveTabName(VERIFICATION);
            } else if (detailsConfig.isTabAccessible) {
                setActiveTabName(DETAILS);
            }
        }
    }, [detailsConfig, verificationConfig, contractConfig, isModalShow, setActiveTabName]);

    const isDetailsActive = useMemo(() => {
        return (
            detailsConfig.isTabAccessible &&
            ([DETAILS, VERIFICATION, CONTRACT] as Array<string>).includes(tabName)
        );
    }, [tabName, detailsConfig.isTabAccessible]);

    const isVerificationActive = useMemo(() => {
        return (
            verificationConfig.isTabAccessible &&
            ([CONTRACT, VERIFICATION] as Array<string>).includes(tabName)
        );
    }, [tabName, verificationConfig.isTabAccessible]);

    const isContractActive = useMemo(() => {
        return contractConfig.isTabAccessible && tabName === CONTRACT;
    }, [tabName, contractConfig.isTabAccessible]);

    return (
        <Nav
            justify
            variant="tabs"
            className="loan-tabs"
            onSelect={handleNavClick}
            activeKey={tabName}
        >
            <span className="nav-line" />

            <Nav.Link eventKey={BASE} active className={clsx(tabName === BASE && 'last-item')}>
                <span className="nav-item-index">1</span> Նոր հայտ
            </Nav.Link>

            <Nav.Link
                eventKey={DETAILS}
                active={isDetailsActive}
                disabled={!detailsConfig.isTabAccessible}
                className={clsx(tabName === DETAILS && 'last-item')}
            >
                <span className="nav-item-index">2</span> Վարկի տվյալներ
            </Nav.Link>

            <Nav.Link
                eventKey={VERIFICATION}
                active={isVerificationActive}
                disabled={!verificationConfig.isTabAccessible}
                className={clsx(tabName === VERIFICATION && 'last-item')}
            >
                <span className="nav-item-index">3</span> Քարտով նույնականացում
            </Nav.Link>

            <Nav.Link
                eventKey={CONTRACT}
                active={isContractActive}
                disabled={!contractConfig.isTabAccessible}
                className={clsx(tabName === CONTRACT && 'last-item')}
            >
                <span className="nav-item-index">4</span> Պայմանագրի տվյալներ
            </Nav.Link>
        </Nav>
    );
}

export default ApplicationTabs;
