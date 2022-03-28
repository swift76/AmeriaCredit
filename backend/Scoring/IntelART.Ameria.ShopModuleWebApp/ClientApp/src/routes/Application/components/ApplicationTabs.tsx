import React from 'react';
import Nav from 'react-bootstrap/Nav';
import { TabsKeyEnum } from 'constants/application';
import { TabsKeysEnumTypes } from 'types';
import { useApplicationDispatch, useApplicationState } from '../Provider';

const { PRE_APPLICATION, APPLICATION_MAIN } = TabsKeyEnum;

function ApplicationTabs() {
    const { setActiveTabName } = useApplicationDispatch();
    const { tabName } = useApplicationState();

    const handleNavClick = (eventKey: string | null) => {
        if (eventKey) {
            setActiveTabName(eventKey as TabsKeysEnumTypes);
        }
    };

    return (
        <Nav
            variant="tabs"
            defaultActiveKey={PRE_APPLICATION}
            onSelect={handleNavClick}
            activeKey={tabName}
        >
            <Nav.Item>
                <Nav.Link eventKey={PRE_APPLICATION}>Սկզբնական</Nav.Link>
            </Nav.Item>
            <Nav.Item>
                <Nav.Link eventKey={APPLICATION_MAIN}>Վարկի պայմաններ</Nav.Link>
            </Nav.Item>
        </Nav>
    );
}

export default ApplicationTabs;
