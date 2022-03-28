import React, { Suspense } from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import Loading from 'components/Loading';
import ConfigProvider from 'providers/ConfigProvider';
import AuthProvider from 'providers/AuthProvider';
import LayoutContainer from 'containers/LayoutContainer';
import 'services/api';
import './styles/index.scss';

ReactDOM.render(
    <Suspense fallback={<Loading className="app-loading" />}>
        <BrowserRouter>
            <AuthProvider>
                <ConfigProvider>
                    <LayoutContainer />
                </ConfigProvider>
            </AuthProvider>
        </BrowserRouter>
    </Suspense>,
    document.getElementById('root')
);
