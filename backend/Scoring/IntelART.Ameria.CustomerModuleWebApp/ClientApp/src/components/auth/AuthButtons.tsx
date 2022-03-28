import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';

function AuthButtons() {
    const { pathname } = useLocation();

    if (pathname === '/reset-password') {
        return null;
    }

    return (
        <div className="auth-form-nav d-flex">
            <NavLink className="btn auth-btn" to="/signin">
                Մուտք համակարգ
            </NavLink>
            <NavLink className="btn auth-btn" to="/signup">
                Գրանցում
            </NavLink>
        </div>
    );
}

export default AuthButtons;
