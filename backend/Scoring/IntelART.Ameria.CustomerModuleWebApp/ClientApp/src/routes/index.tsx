import React, { lazy } from 'react';
import { useRoutes, Navigate } from 'react-router-dom';

const Profile = lazy(() => import('./Profile'));
const Application = lazy(() => import('./Application'));
const AppliactoinList = lazy(() => import('./AppliactoinList'));

export const routes = [
    {
        path: '',
        element: <AppliactoinList />
    },
    {
        path: 'application',
        element: <Application />
    },
    {
        path: 'application/:id',
        element: <Application />
    },
    {
        path: 'profile',
        element: <Profile />
    },
    {
        path: '*',
        element: <Navigate to="/" replace />
    }
];

function AppRoutes() {
    const appRoutes = useRoutes(routes);

    return appRoutes;
}

export default AppRoutes;
