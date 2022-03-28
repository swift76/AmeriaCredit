/* eslint-disable react-hooks/exhaustive-deps */
import React, { createContext, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useApplications } from 'hooks';
import PageContainer from 'containers/PageContainer';
import Table from './components/Table';
import ActionModal from './components/ActionModal';

export const ApplicationListContext = createContext<any>(undefined);

const initialState = { id: null, type: null };

function AppliactoinList() {
    const [state, setState] = useState<any>(initialState);

    const [intervalMs, setIntervalMs] = useState(5000);
    const { data = [], isLoading, refetch } = useApplications({
        refetchInterval: intervalMs
    });

    useEffect(() => {
        refetch();
    }, []);

    useEffect(() => {
        return () => {
            setIntervalMs(0);
        };
    }, []);

    const onCloseModal = () => {
        setState(initialState);
    };

    return (
        <PageContainer
            title="Հայտեր"
            action={
                <Link to="/application" className="icon-link">
                    <i className="icon am-icon-add" /> Դիմել նոր վարկի
                </Link>
            }
        >
            <ApplicationListContext.Provider value={{ state, setState }}>
                <Table data={data} loading={isLoading} />
                <ActionModal
                    show={!!state.id}
                    id={state.id}
                    type={state.type}
                    onClose={onCloseModal}
                />
            </ApplicationListContext.Provider>
        </PageContainer>
    );
}

export default AppliactoinList;
