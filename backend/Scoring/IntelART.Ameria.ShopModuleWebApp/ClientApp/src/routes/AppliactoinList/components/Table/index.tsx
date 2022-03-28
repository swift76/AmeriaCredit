import React, { useMemo } from 'react';
import { Table, Row, Col } from 'react-bootstrap';
import { useTable, usePagination, useSortBy, useFilters } from 'react-table';
import ReactPaginate from 'react-paginate';
import { UseFormMethods } from 'react-hook-form';
import { DatePickerInput, SearchField, SelectField } from 'components/Form';
import { APP_TABLE_LENGTH_OPTIONS } from 'constants/applicationsTable';
import clsx from 'clsx';
import { IApplications } from 'types';
import Loading from 'components/Loading';
import LoadingOverlay from 'components/LoadingOverlay';
import { columnsConfig } from './Config';
import TableRow from './Row';

type DateFilter = {
    dateFrom?: string;
    dateTo?: string;
};

export interface Props {
    data: IApplications[];
    loading?: boolean;
    onSetDateFilter: React.Dispatch<React.SetStateAction<DateFilter>>;
    dateFilter: DateFilter;
}

function ApplicationsTable({ data, loading, onSetDateFilter, dateFilter }: Props) {
    const columns = useMemo(() => columnsConfig, []);

    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        prepareRow,
        page,
        setFilter,
        pageCount,
        gotoPage,
        setPageSize,
        state: { pageSize }
    } = useTable(
        {
            columns,
            data,
            initialState: { pageSize: 10 }
        },
        useFilters,
        useSortBy,
        usePagination
    );

    const handleDateFilterChange: UseFormMethods['setValue'] = (name, date) => {
        onSetDateFilter(prev => ({ ...prev, [name]: date }));
        localStorage.setItem(`admin_loan_list_${name}`, date as any);
    };

    const handlePageClick = ({ selected }: { selected: number }) => {
        gotoPage(selected);
    };

    const firstItem = (page[0] && Number(page[0].id) + 1) ?? 0;
    const lastItem = (page[0] && Number(page[page.length - 1].id) + 1) ?? 0;

    return (
        <div className="table-wrapper applications-table">
            <Row>
                <Col sm={3}>
                    <div className="d-flex justify-content-around align-items-center">
                        <span className="form-group">Ցուցադրել</span>
                        <SelectField
                            options={APP_TABLE_LENGTH_OPTIONS}
                            value={pageSize}
                            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                                setPageSize(Number(event.target.value));
                            }}
                            noChoiceOption
                        />
                        <span className="form-group">Գրանցում</span>
                    </div>
                </Col>

                <Col sm={2}>
                    <DatePickerInput
                        setValue={handleDateFilterChange}
                        name="dateFrom"
                        value={dateFilter.dateFrom}
                        maxDate={new Date()}
                    />
                </Col>
                <Col sm={2}>
                    <DatePickerInput
                        setValue={handleDateFilterChange}
                        name="dateTo"
                        value={dateFilter.dateTo}
                        maxDate={new Date()}
                    />
                </Col>

                <Col sm={5}>
                    <SearchField
                        name="application_search"
                        onChange={(event: React.ChangeEvent<HTMLInputElement>) =>
                            setFilter('name', event.target.value)
                        }
                        placeholder="Որոնում"
                    />
                </Col>
            </Row>
            <Table striped hover className="border-bottom" {...getTableProps()}>
                <thead>
                    <tr className="ws-nowrap">
                        {headerGroups[0].headers.map((column: any) => (
                            <th
                                {...column.getHeaderProps(column.getSortByToggleProps())}
                                title={column.title}
                            >
                                {column.render('Header')}
                                <span
                                    className={
                                        column.isSorted
                                            ? clsx('order', {
                                                  desc: column.isSortedDesc,
                                                  asc: !column.isSortedDesc
                                              })
                                            : ''
                                    }
                                />
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody {...getTableBodyProps()}>
                    {!page.length && !loading ? (
                        <tr>
                            <td colSpan={5} className="text-center">
                                Տվյալ պահին չկան գրանցված հայտեր
                            </td>
                        </tr>
                    ) : (
                        page.map((row: any) => {
                            prepareRow(row);
                            return (
                                <TableRow
                                    key={row.id}
                                    data={row.original}
                                    rowProps={row.getRowProps()}
                                />
                            );
                        })
                    )}
                    {loading && (
                        <tr>
                            <td colSpan={5} className="text-center">
                                <LoadingOverlay className="table-loading-overlay">
                                    <Loading />
                                </LoadingOverlay>
                            </td>
                        </tr>
                    )}
                </tbody>
            </Table>

            <div className="d-flex justify-content-between">
                <span>{`Ցուցադրված է ${firstItem} -ից ${lastItem} / ${data.length} գրանցում`}</span>
                <ReactPaginate
                    previousLabel="Նախորդ"
                    nextLabel="Հաջորդ"
                    breakLabel="..."
                    breakClassName="break-me"
                    pageCount={pageCount}
                    marginPagesDisplayed={1}
                    pageRangeDisplayed={2}
                    onPageChange={handlePageClick}
                    containerClassName="pagination"
                    activeClassName="active"
                />
            </div>
        </div>
    );
}

export default ApplicationsTable;
