/* eslint-disable react/no-danger */
import React, { useCallback, useEffect, useState, useMemo } from 'react';
import { useForm, FormProvider, Controller } from 'react-hook-form';
import { IApplicationVerification, ISelectOption } from 'types';
import { yupResolver } from '@hookform/resolvers';
import LoadingButton from 'components/LoadingButton';
import { Form, Row, Col, Button, Alert, Modal } from 'react-bootstrap';
import { useApplication, useApplicationStatusText } from 'hooks';
import { useQueryCache } from 'react-query';

import { TextField, SelectField, RestrictedInput } from 'components/Form';
import { FieldSet } from 'components/fieldset';
import { getMonthOptions, getExpiryYearOptions, toUtcMidnight } from 'helpers/date';
import { validateCard, checkCreditCardAuthorization, creditCardAuthorization } from 'queryies';
import { applicationVerificationScheme as scheme } from '../validations';
import { useApplicationDispatch } from '../Provider';
import StatusBadge from '../components/StatusBadge';

type Props = {
    id: string;
    disabled: boolean;
};

type Error = { isError: boolean; message?: string };

function ApplicationVerification({ id, disabled }: Props) {
    const queryCache = useQueryCache();
    const { setActiveTabName } = useApplicationDispatch();
    const { data: applicationData } = useApplication(id, {
        enabled: false
    });
    const [error, setError] = useState<Error>({
        isError: false
    });
    const [modalError, setModalError] = useState<Error>({
        isError: false
    });
    const [showModal, setShowModal] = useState<boolean>(false);
    const [isSmsSending, setIsSmsSending] = useState<boolean>(false);
    const [modalSmsCode, setModalSmsCode] = useState<string>('');
    const [isModalContinuseDisabled, setIsModalContinuseDisabled] = useState<boolean>(false);

    const applicationId = applicationData?.ID;

    const { statusAm: statusText, statusState } = useApplicationStatusText(applicationId, {
        enabled: !!applicationId
    });

    const methods = useForm({
        resolver: yupResolver(scheme)
    });
    const { handleSubmit, errors, control } = methods;

    const monthOptions = useMemo<ISelectOption[]>(() => {
        return getMonthOptions();
    }, []);

    const yearOptions = useMemo<ISelectOption[]>(() => {
        return getExpiryYearOptions();
    }, []);

    useEffect(() => {
        if (errors.CARD_NUMBER && errors.CARD_NUMBER.type === 'len') {
            setError({ isError: true, message: errors.CARD_NUMBER.message });
        } else {
            setError({ isError: false });
        }
    }, [errors.CARD_NUMBER]);

    const onSubmit = useCallback(
        async (values: IApplicationVerification) => {
            const date = new Date(values.EXPIRY_YEAR, values.EXPIRY_MONTH, 0);
            const cardObj = {
                cardNumber: values.CARD_NUMBER,
                expiryDate: toUtcMidnight(date)
            };

            try {
                await validateCard({ ...cardObj, ID: applicationId ?? undefined });
                setShowModal(true);
            } catch (err) {
                setError({ isError: true, message: err.Message });
            }
        },
        [applicationId]
    );

    const handleModalContinue = async () => {
        setModalError({ isError: false });

        if (modalSmsCode.length === 0) {
            setModalError({ isError: true, message: 'Խնդրում ենք լրացնել կոդը։' });
        } else if (applicationId) {
            try {
                await checkCreditCardAuthorization(applicationId, modalSmsCode);
                queryCache.invalidateQueries(['APPLICATION', id]);
                setActiveTabName('CONTRACT');
            } catch (err) {
                if (err.ErrorCode === 'ERR-0006') {
                    setIsModalContinuseDisabled(true);
                }
                setModalError({ isError: true, message: err.Message });
            }
        }
    };

    const handleSendSMS = async () => {
        if (applicationId) {
            setModalError({ isError: false });
            setIsSmsSending(true);

            try {
                await creditCardAuthorization(applicationId);
            } catch (err) {
                if (err.ErrorCode === 'ERR-0008') {
                    setIsModalContinuseDisabled(true);
                }
                setModalError({ isError: true, message: err.Message });
            }

            setIsSmsSending(false);
        }
    };

    const handleModalInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { value } = event.target;
        setModalSmsCode(prev => (value.length > 6 ? prev : value));
    };

    return (
        <FieldSet className="application-form-wrapper" disabled={disabled}>
            <FormProvider {...methods}>
                <Form onSubmit={handleSubmit(onSubmit)} id="VerificationForm">
                    <Row className="mb-3">
                        <Col sm={12}>
                            <h2 className="fieldset-title">
                                Քարտով նույնականացում{' '}
                                {disabled && (
                                    <StatusBadge status={statusState ?? ''}>
                                        {statusText}
                                    </StatusBadge>
                                )}
                            </h2>
                            <p>
                                Վարկը տրամադրելու համար խնդրում ենք լրացնել Ձեր քարտի տվյալները։
                                Քարտի տվյալներն անհրաժեշտ են Ձեզ նույնականացնելու և վարկային
                                պայմանագիրը կնքելու համար։
                            </p>
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={RestrictedInput}
                                control={control}
                                name="CARD_NUMBER"
                                label="Մուտքագրեք քարտի համարը"
                                type="number"
                                error={
                                    errors.CARD_NUMBER &&
                                    errors.CARD_NUMBER.type === 'required' &&
                                    errors.CARD_NUMBER
                                }
                                maxLength={16}
                            />
                        </Col>
                        <Col sm={6}>
                            <Form.Row>
                                <Col sm={12}>
                                    <Form.Label>Վավերականության ժամկետը</Form.Label>
                                </Col>
                                <Col sm={4}>
                                    <Controller
                                        as={SelectField}
                                        control={control}
                                        name="EXPIRY_MONTH"
                                        error={errors.EXPIRY_MONTH}
                                        options={monthOptions}
                                    />
                                </Col>
                                <Col sm={4}>
                                    <Controller
                                        as={SelectField}
                                        control={control}
                                        name="EXPIRY_YEAR"
                                        error={errors.EXPIRY_YEAR}
                                        options={yearOptions}
                                    />
                                </Col>
                            </Form.Row>
                        </Col>
                    </Row>
                </Form>
                <div className="form-actions">
                    <LoadingButton
                        type="submit"
                        variant="primary"
                        form="VerificationForm"
                        loading={false}
                    >
                        Հաստատել
                    </LoadingButton>
                </div>
                {error.isError && (
                    <>
                        <br />
                        <Alert variant="danger">
                            <div
                                dangerouslySetInnerHTML={{
                                    __html: error.message || 'Տեղի է ունեցել սխալ'
                                }}
                            />
                        </Alert>
                    </>
                )}
                <Modal show={showModal} onHide={() => setShowModal(false)}>
                    <Modal.Header closeButton>
                        <Modal.Title>
                            Խնդրում ենք մուտքագրել Ձեր՝ Բանկում գրանցված բջջային հեռախոսահամարին
                            ուղարկված գաղտնաբառը։
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <TextField
                            label="Մուտքագրեք նույնականացման կոդը"
                            type="text"
                            onChange={handleModalInputChange}
                            value={modalSmsCode}
                        />
                        {modalError.isError && (
                            <Alert variant="danger">
                                <div
                                    dangerouslySetInnerHTML={{
                                        __html: modalError.message || 'Տեղի է ունեցել սխալ'
                                    }}
                                />
                            </Alert>
                        )}
                    </Modal.Body>
                    <Modal.Footer>
                        <LoadingButton
                            variant="outline-primary"
                            onClick={handleSendSMS}
                            loading={isSmsSending}
                            disabled={isModalContinuseDisabled}
                        >
                            Կրկին ուղարկել
                        </LoadingButton>
                        <Button
                            variant="primary"
                            onClick={handleModalContinue}
                            disabled={isModalContinuseDisabled}
                        >
                            Շարունակել
                        </Button>
                    </Modal.Footer>
                </Modal>
            </FormProvider>
        </FieldSet>
    );
}

export default ApplicationVerification;
