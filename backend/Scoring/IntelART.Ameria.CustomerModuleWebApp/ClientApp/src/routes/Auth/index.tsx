import React, { lazy, Suspense } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Logo from 'components/Logo';
import Loading from 'components/Loading';
import AuthButtons from 'components/auth/AuthButtons';
import { Navbar } from 'react-bootstrap';

const Login = lazy(() => import('./pages/Login'));
const Register = lazy(() => import('./pages/Register'));
const ResetPassword = lazy(() => import('./pages/ResetPassword'));

function Auth() {
    return (
        <main className="main-layout">
            <Navbar collapseOnSelect expand="lg" fixed="top" bg="white" className="page-navbar">
                <Navbar.Brand>
                    <Logo />
                </Navbar.Brand>
            </Navbar>
            <div className="auth-page">
                <div className="auth-form-wrapper">
                    <AuthButtons />
                    <Suspense fallback={<Loading className="page-loading" />}>
                        <Routes>
                            <Route path="/signin" element={<Login />} />
                            <Route path="/signup" element={<Register />} />
                            <Route path="/reset-password" element={<ResetPassword />} />
                            <Route path="*" element={<Navigate to="/signin" replace />} />
                        </Routes>
                    </Suspense>
                </div>
            </div>
        </main>
    );
}

export default Auth;
