import React, { lazy, useEffect } from 'react';
import { useRoutes, Navigate } from 'react-router-dom';

const Application = lazy(() => import('./Application'));
const AppliactoinList = lazy(() => import('./AppliactoinList'));
const ResetPassword = lazy(() => import('./ResetPassword'));

export const routes = [
    {
        path: '',
        element: <AppliactoinList />
    },
    {
        path: 'NewApplication',
        element: <Application />
    },
    {
        path: 'application/:id',
        element: <Application />
    },
    {
        path: '/Account/ChangePassword',
        element: <ResetPassword />
    },
    {
        path: '*',
        element: <Navigate to="/" replace />
    }
];

function AppRoutes() {
    const appRoutes = useRoutes(routes);

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [appRoutes]);

    return appRoutes;
}

export default AppRoutes;
