/* eslint-disable react-hooks/exhaustive-deps */
import React, { createContext, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useApplications, useUserRole } from 'hooks';
import { toUtcMidnight } from 'helpers/date';
import PageContainer from 'containers/PageContainer';
import { IApplications } from 'types';
import { USER_ROLES } from 'constants/application';
import Table from './components/Table';
import ActionModal from './components/ActionModal';

export const ApplicationListContext = createContext<any>(undefined);

const initialState = { id: null, type: null };

function AppliactoinList() {
    const [state, setState] = useState<any>(initialState);
    const [tableData, setTableData] = useState<IApplications[]>([]);
    const [intervalMs, setIntervalMs] = useState(5000);
    const [dateFilter, setDateFilter] = useState<{ dateFrom?: string; dateTo?: string }>({
        dateFrom: localStorage.getItem('admin_loan_list_dateFrom') ?? toUtcMidnight(new Date()),
        dateTo: localStorage.getItem('admin_loan_list_dateTo') ?? toUtcMidnight(new Date())
    });

    const { data, isLoading } = useApplications(dateFilter.dateFrom, dateFilter.dateTo, {
        refetchInterval: intervalMs,
        enabled: !!dateFilter.dateFrom && !!dateFilter.dateTo
    });

    const { data: userRole } = useUserRole();

    const onCloseModal = () => {
        setState(initialState);
    };

    useEffect(() => {
        if (data) {
            setTableData(data);
        }
    }, [data]);

    useEffect(() => {
        return () => {
            setIntervalMs(0);
        };
    }, []);

    return (
        <PageContainer>
            <ApplicationListContext.Provider value={{ state, setState }}>
                <div className="application-list-container">
                    <div className="d-flex justify-content-between">
                        <h1>Հայտերի ցուցակ</h1>
                        {userRole === USER_ROLES.USER && (
                            <Link to="NewApplication" id="newApplication" className="add">
                                Նոր հայտի գրանցում
                            </Link>
                        )}
                    </div>
                    <Table
                        data={tableData}
                        loading={isLoading}
                        dateFilter={dateFilter}
                        onSetDateFilter={setDateFilter}
                    />
                    <ActionModal
                        show={!!state.id}
                        id={state.id}
                        type={state.type}
                        onClose={onCloseModal}
                    />
                </div>
            </ApplicationListContext.Provider>
        </PageContainer>
    );
}

export default AppliactoinList;
