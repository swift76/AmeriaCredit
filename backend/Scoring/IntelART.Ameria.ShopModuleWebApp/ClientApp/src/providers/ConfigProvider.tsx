import React from 'react';
import 'configs/yupLocalConfig';
import 'configs/datepickerLocale';
import { ToastContainer } from 'react-toastify';

type Props = {
    children: React.ReactNode;
};

if (!navigator.platform.includes('Mac')) {
    document.documentElement.classList.add('am-scrollbar');
}

function ConfigProvider({ children }: Props) {
    return (
        <>
            {children}
            <ToastContainer
                position="top-right"
                autoClose={3000}
                hideProgressBar
                newestOnTop={false}
                closeOnClick
                pauseOnHover
            />
        </>
    );
}

export default ConfigProvider;
