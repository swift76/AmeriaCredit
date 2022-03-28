import React, { useState } from 'react';
import { Row, Col, Button, Alert } from 'react-bootstrap';
import { v4 as uuidv4 } from 'uuid';
import { TextField, NumberFormatField } from 'components/Form';
import { IProduct } from 'types';
import { numberWithCommas } from 'helpers/data';

type Props = {
    onAddProduct: (product: IProduct) => void;
    onDeleteProduct: (id: string) => void;
    products: IProduct[];
    totalPrice: number;
    error: boolean;
};

function ProductDetails({ onAddProduct, onDeleteProduct, products, totalPrice, error }: Props) {
    const [fields, setFields] = useState({ DESCRIPTION: '', QUANTITY: 0, PRICE: 0 });
    const [fieldErrors, setFieldsErrors] = useState({
        DESCRIPTION: false,
        QUANTITY: false,
        PRICE: false,
        message: 'Պարտադիր լրացման դաշտ'
    });

    const handleInput = (name: string, value: string) => {
        setFields(prev => ({ ...prev, [name]: value }));
    };

    const handleAddProduct = () => {
        let isError = false;
        Object.keys(fields).forEach(key => {
            const field = fields[key as keyof typeof fields];
            const err = (typeof field === 'string' && field.length === 0) || Number(field) === 0;
            setFieldsErrors(prev => ({ ...prev, [key]: err }));

            if (err) isError = true;
        });

        if (!isError) {
            onAddProduct({ ...fields, ID: uuidv4() });
            setFields({ DESCRIPTION: '', QUANTITY: 0, PRICE: 0 });
        }
    };

    return (
        <>
            <Row className="mb-3">
                <Col sm={12}>
                    <h2 className="fieldset-title">Ապրանքի տվյալներ</h2>
                </Col>
            </Row>
            <div className="product-details mb-5">
                <Row className="mb-1">
                    <Col sm={6}>
                        <TextField
                            label="Ապրանքի անվանում"
                            name="DESCRIPTION"
                            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                                handleInput('DESCRIPTION', event.target.value);
                            }}
                            error={
                                fieldErrors.DESCRIPTION
                                    ? { ...fieldErrors, type: 'manual' }
                                    : undefined
                            }
                            value={fields.DESCRIPTION}
                        />
                    </Col>
                    <Col sm={2}>
                        <NumberFormatField
                            label="Քանակ"
                            name="QUANTITY"
                            setValue={handleInput}
                            error={fieldErrors.QUANTITY && fieldErrors}
                            allowNegative={false}
                            decimalScale={0}
                            value={fields.QUANTITY}
                        />
                    </Col>
                    <Col sm={2}>
                        <NumberFormatField
                            label="Գին"
                            name="PRICE"
                            setValue={handleInput}
                            error={fieldErrors.PRICE && fieldErrors}
                            allowNegative={false}
                            value={fields.PRICE}
                        />
                    </Col>
                    <Col sm={2} className="d-flex ">
                        <Button variant="primary" className="add-button" onClick={handleAddProduct}>
                            Ավելացնել
                        </Button>
                    </Col>
                </Row>
                {error && <Alert variant="danger">Պարտադիր լրացման դաշտ</Alert>}
                <div className="products">
                    {products.map((product, index) => (
                        <Row className="product-row" key={product.ID}>
                            <Col sm={7}>{product.DESCRIPTION}</Col>
                            <Col sm={2}>{product.QUANTITY}</Col>
                            <Col sm={2}>{`${numberWithCommas(product.PRICE)} դր.`}</Col>
                            <Col sm={1}>
                                <Button
                                    variant="link"
                                    onClick={() => onDeleteProduct(product.ID)}
                                    className="delete-button"
                                >
                                    <i className="am-icon-cross" />
                                </Button>
                            </Col>
                        </Row>
                    ))}
                </div>
                <div className="total mb-3">{`Ապրանքների ընդհանուր գումար ${numberWithCommas(
                    totalPrice
                )} դր.`}</div>
            </div>
        </>
    );
}

export default ProductDetails;
