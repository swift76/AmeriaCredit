import React, { useCallback, useState } from 'react';
import { Nav, Navbar, Spinner } from 'react-bootstrap';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuthDispatch } from 'providers/AuthProvider';
import Logo from 'components/Logo';

function Header() {
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
                    <Nav.Item>
                        <Nav.Link
                            onClick={() => {
                                navigate('/Account/ChangePassword');
                            }}
                            className="outline-none"
                        >
                            Փոխել գաղտնաբառը
                        </Nav.Link>
                    </Nav.Item>
                    <span className="nav-link no-mobile">/</span>
                    <Nav.Item>
                        <Nav.Link
                            onClick={handleLogout}
                            disabled={logoutLoading}
                            className="outline-none"
                        >
                            {logoutLoading ? (
                                <Spinner animation="border" size="sm" variant="primary" />
                            ) : (
                                <span>Դուրս գալ</span>
                            )}
                        </Nav.Link>
                    </Nav.Item>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
}

export default Header;
