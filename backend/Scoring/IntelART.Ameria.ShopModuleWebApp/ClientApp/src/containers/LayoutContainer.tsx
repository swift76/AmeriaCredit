import React from 'react';
import { useAuthState } from 'providers/AuthProvider';
import Main from 'containers/MainContainer';
import Auth from 'routes/Auth';

const LayoutContainer: React.FC = () => {
    const { isAutentificated } = useAuthState();

    return isAutentificated ? <Main /> : <Auth />;
};

export default LayoutContainer;
