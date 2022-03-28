import React, { lazy, Suspense } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Loading from 'components/Loading';

const Login = lazy(() => import('./Login'));

function Auth() {
    return (
        <main className="main-layout">
            <div className="auth-page">
                <div className="auth-form-wrapper">
                    <Suspense fallback={<Loading className="page-loading" />}>
                        <Routes>
                            <Route path="/signin" element={<Login />} />
                            <Route path="*" element={<Navigate to="/signin" replace />} />
                        </Routes>
                    </Suspense>
                </div>
            </div>
        </main>
    );
}

export default Auth;
