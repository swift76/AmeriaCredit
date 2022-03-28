import React, { useMemo } from 'react';
import { Table } from 'react-bootstrap';
import clsx from 'clsx';
import { useTableSort } from 'hooks';
import { IApplications } from 'types';
import Loading from 'components/Loading';
import { columnsConfig } from './Config';
import TableRow from './Row';

export interface Props {
    data: IApplications[];
    loading?: boolean;
}

function ApplicationsTable({ data, loading }: Props) {
    const { createSortHandler, sortedRows, order, orderBy } = useTableSort<
        IApplications,
        keyof IApplications
    >(data, 'CREATION_DATE');

    const columns = useMemo(() => columnsConfig, []);

    return (
        <div className="table-wrapper applications-table">
            <Table striped hover>
                <thead>
                    <tr className="ws-nowrap">
                        {columns.map(({ accessor, header }, index) => {
                            const isLastCell = index === columns.length - 1;
                            const orderByClass = orderBy === accessor;
                            return (
                                <th
                                    key={accessor}
                                    onClick={createSortHandler(accessor)}
                                    className={clsx({ 'text-right': isLastCell })}
                                >
                                    {header}
                                    <span className={clsx('order', { [order]: orderByClass })} />
                                </th>
                            );
                        })}
                    </tr>
                </thead>
                <tbody>
                    {!sortedRows.length && !loading ? (
                        <tr>
                            <td colSpan={5} className="text-center">
                                Տվյալ պահին չկան գրանցված հայտեր
                            </td>
                        </tr>
                    ) : (
                        sortedRows.map(row => <TableRow key={row.ID} data={row} />)
                    )}
                </tbody>
            </Table>
            {loading && (
                <div className="table-loading-overlay">
                    <Loading />
                </div>
            )}
        </div>
    );
}

export default ApplicationsTable;
