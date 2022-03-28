import React, { useCallback, useState } from 'react';
import { Nav, Navbar, Spinner } from 'react-bootstrap';
import { useLocation, useNavigate, Link } from 'react-router-dom';
import { useAuthDispatch } from 'providers/AuthProvider';
import { useUserState } from 'providers/UserProvider';
import Logo from 'components/Logo';

function Header() {
    const userData = useUserState();
    const { logout } = useAuthDispatch();
    const { pathname } = useLocation();
    const navigate = useNavigate();
    const [logoutLoading, setLogoutLoading] = useState(false);

    const handleLogout = async () => {
        setLogoutLoading(true);
        try {
            await logout();
        } finally {
            setLogoutLoading(false);
        }
    };

    const handleLogoClick = useCallback(() => {
        if (pathname !== '/signin') {
            navigate('/');
        }
    }, [pathname, navigate]);

    return (
        <Navbar collapseOnSelect expand="lg" fixed="top" bg="white" className="page-navbar">
            <Navbar.Brand onClick={handleLogoClick}>
                <Logo />
            </Navbar.Brand>
            <Navbar.Toggle aria-controls="navbar-nav" />
            <Navbar.Collapse id="navbar-nav">
                <Nav className="ml-auto">
                    <Nav.Link as={Link} to="/profile">
                        {`${userData?.FIRST_NAME_EN || ''} ${userData?.LAST_NAME_EN || ''}`}
                    </Nav.Link>
                    <Nav.Item>
                        <Nav.Link
                            onClick={handleLogout}
                            disabled={logoutLoading}
                            className="outline-none"
                        >
                            {logoutLoading ? (
                                <Spinner animation="border" size="sm" variant="primary" />
                            ) : (
                                <i className="am-icon-logout" />
                            )}
                        </Nav.Link>
                    </Nav.Item>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
}

export default Header;
