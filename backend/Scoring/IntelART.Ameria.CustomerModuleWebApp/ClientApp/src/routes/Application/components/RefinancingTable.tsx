import React from 'react';
import { Row, Col, Table, Spinner, Alert } from 'react-bootstrap';
import { useRefinancingLoan } from 'hooks';
import { TextField } from 'components/Form';
import { useFormContext } from 'react-hook-form';
import { format } from 'date-fns';
import { DATE_FORMAT_DEFAULT } from 'constants/date';

type Props = {
    id: string;
};

export default function RefinancingTable({ id }: Props) {
    const { register, errors } = useFormContext();

    const { data: refinancingData } = useRefinancingLoan(id, {
        enabled: !!id
    });

    return (
        <Row className="mb-3">
            <Alert variant="warning">
                <h4>Ուշադրություն`</h4>
                Վարկային գծերի և օվերդրաֆտների վերաֆինանսավորման պարագայում <b>ՊԱՐՏԱԴԻՐ</b>{' '}
                համոզվեք, որ վերաֆինանսավորվող վարկային պայմանագիրը վերջնական փակվել է:
                <br />
                Վերաֆինանսավորվող պայմանագիրը 30 օրվա ընթացքում փակ չլինելու դեպքում Ամերիաբանկի
                կողմից վերաֆինանսավորման նպատակով ձեզ տրամադրված վարկային գծի/օվերդրաֆտի
                տոկոսադրույքը կվերանայվի <b>+2%</b>-ով:
                <br />
            </Alert>
            <Col sm={12}>
                <h2 className="fieldset-title">Վերաֆինանսավորում </h2>
            </Col>
            <Table striped bordered hover className="refinancing-table" size="sm">
                <thead>
                    <tr>
                        <th>Բանկ</th>
                        <th>Տոկոսադրույք</th>
                        <th>Սկզբնական գումար</th>
                        <th>Վարկի մնացորդ</th>
                        <th>Տրամադրման ամսաթիվ</th>
                        <th>Մարման ամսաթիվ</th>
                        <th>Վարկի կոդ</th>
                    </tr>
                </thead>
                <tbody>
                    {refinancingData && refinancingData.length !== 0 ? (
                        refinancingData.map((field, index) => (
                            <tr key={field.ROW_ID}>
                                <td>{field.ORIGINAL_BANK_NAME}</td>
                                <td>{`${field.INITIAL_INTEREST} %`}</td>
                                <td>{field.INITIAL_AMOUNT}</td>
                                <td>{field.CURRENT_BALANCE}</td>
                                <td>
                                    {format(
                                        new Date(field.DRAWDOWN_DATE || ''),
                                        DATE_FORMAT_DEFAULT
                                    )}
                                </td>
                                <td>
                                    {format(
                                        new Date(field.MATURITY_DATE || ''),
                                        DATE_FORMAT_DEFAULT
                                    )}
                                </td>
                                <td>
                                    <TextField
                                        ref={register()}
                                        name={`LOAN_CODES[${index}].LOAN_CODE`}
                                        error={
                                            errors.LOAN_CODES &&
                                            errors?.LOAN_CODES[index]?.LOAN_CODE
                                        }
                                    />
                                    <input
                                        ref={register()}
                                        type="hidden"
                                        name={`LOAN_CODES[${index}].ROW_ID`}
                                        value={field.ROW_ID || ''}
                                    />
                                    <input
                                        ref={register()}
                                        type="hidden"
                                        name={`LOAN_CODES[${index}].APPLICATION_ID`}
                                        value={id}
                                    />
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={7} style={{ textAlign: 'center' }}>
                                <Spinner animation="border" role="status" variant="primary" />
                            </td>
                        </tr>
                    )}
                </tbody>
            </Table>
        </Row>
    );
}
