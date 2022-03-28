import React from 'react';
import { Nav } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';

const items = [
    { title: 'Հայտեր', path: '/' },
    { title: 'Դիմել նոր վարկի', path: '/application' },
    { title: 'Անձնական տվյալներ', path: '/profile' }
];

const Sidebar: React.FC = () => {
    const { pathname } = useLocation();

    const activeKey = React.useMemo(() => {
        if (pathname.search('/application') >= 0) {
            return '/application';
        }
        return pathname;
    }, [pathname]);

    return (
        <Nav
            defaultActiveKey="/"
            activeKey={activeKey}
            className="flex-column navigation sticky-top"
        >
            {items.map(({ title, path }) => (
                <Nav.Item key={title}>
                    <Nav.Link eventKey={path} to={path} as={Link}>
                        {title}
                    </Nav.Link>
                </Nav.Item>
            ))}
        </Nav>
    );
};

export default Sidebar;
