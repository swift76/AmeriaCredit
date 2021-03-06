/* eslint-disable react-hooks/exhaustive-deps */
import React, { useState, useEffect, useMemo } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useForm, FormProvider, Controller } from 'react-hook-form';
import differenceInMinutes from 'date-fns/differenceInMinutes';
import { useUserState } from 'providers/UserProvider';
import locales from 'locales';
import Api from 'services/api';
import {
    useLoanTypes,
    useActiveCards,
    useCreditCardTypes,
    useApplication,
    useBankBranches,
    useApplicationAgreed,
    useApplicationStatusText
} from 'hooks';

import { IApplicationAgreed } from 'types';
import { yupResolver } from '@hookform/resolvers';
import { Button, Form, Row, Col, Modal, Alert, Spinner } from 'react-bootstrap';
import LoadingButton from 'components/LoadingButton';
import { saveApplicationAgree } from 'queryies';
import { termsAndTariffLink, rulsLink } from 'constants/links';
import { TextField, SelectField, NumberFormatField } from 'components/Form';
import { FieldSet } from 'components/fieldset';
import { buildSelectOption, getDocumentUrl, openFile } from 'helpers/data';
import ContractModalContent from '../components/ContractModalContent';
import StatusBadge from '../components/StatusBadge';
import { applicationContractScheme } from '../validations';

const { receiveLoanByExistingCard, receiveLoanByNewCard, actualInterestUserText } = locales;

type Props = {
    id: string;
    disabled: boolean;
};

function ApplicationContract({ id, disabled }: Props) {
    const navigate = useNavigate();
    const [loanInterest2, setLoanIntetest2] = useState<{ value: null | string; loading: boolean }>({
        value: null,
        loading: false
    });
    const [actualIntConfirmText, setActualIntConfirmText] = useState('');
    const [confirmActualInterest, setConfirmActualInterest] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [pollingTime, setPollingTime] = useState<Date | null>(null);
    const [errorCode, setErrorCode] = useState<null | string>(null);
    const [submitting, setSubmitting] = useState(false);
    const profileData = useUserState();
    const [refetchInterval, setRefechInterval] = useState<number | false | undefined>(false);

    const [validatationContext, setValidationContext] = useState({ isCardAccount: false });
    const methods = useForm({
        resolver: yupResolver(applicationContractScheme),
        context: validatationContext
    });
    const [IsAgreementConfirmed, setIsAgreemenConfirmed] = useState(false);

    const { handleSubmit, errors, register, control, reset, watch, getValues, setValue } = methods;

    // useQueryies
    const isStudent = profileData?.IS_STUDENT;
    const { data: loanTypes } = useLoanTypes(isStudent, { enabled: isStudent !== undefined });

    const { data: bankBranches, isLoading: isBankBranchesLoading } = useBankBranches();

    const { data: applicationAgreedData } = useApplicationAgreed(id, {
        enabled: !!id
    });

    const { data: applicationData, updatedAt } = useApplication(id, {
        refetchInterval,
        enabled: !!refetchInterval
    });

    const { statusAm: statusText, statusState } = useApplicationStatusText(id, {
        enabled: !!id
    });

    const loanTypeId = applicationData?.LOAN_TYPE_ID;
    const statusID = applicationData?.STATUS_ID;
    const firstName = applicationData?.FIRST_NAME;
    const lastName = applicationData?.LAST_NAME;
    const finalAmount = applicationAgreedData?.FINAL_AMOUNT;
    const IsAgreementNeeded = applicationAgreedData?.IsAgreementNeeded;
    const agremmentValues = watch('agreement');

    // form watching fields
    const isNewCard = watch('IS_NEW_CARD');
    const isCartDelivery = watch('IS_CARD_DELIVERY');
    const agreedWithTerms = watch('AGREED_WITH_TERMS');

    const startPolling = () => {
        setRefechInterval(10000);
        setPollingTime(new Date());
        setShowModal(true);
    };

    const loanType = useMemo(() => {
        return loanTypes?.find(({ CODE }) => CODE === loanTypeId);
    }, [loanTypes, loanTypeId]);

    const isCardAccount = loanType?.IS_CARD_ACCOUNT;

    const { data: activeCards, isLoading: isActiveCardsLoading } = useActiveCards(id, {
        enabled: !!id && isCardAccount
    });

    const { data: creditCardTypes, isLoading: isCreditCardTypesLoading } = useCreditCardTypes(id, {
        enabled: !!id && isCardAccount
    });

    const hasActiveCards = !!activeCards?.length;

    const creditCardTypeOptions = useMemo(() => {
        return buildSelectOption(creditCardTypes || []);
    }, [creditCardTypes]);

    const bankBranchesOptions = useMemo(() => {
        return buildSelectOption(bankBranches || []);
    }, [bankBranches]);

    const activeCardsOptions = useMemo(() => {
        return activeCards
            ? activeCards.map(({ CardNumber, CardDescription }) => ({
                  value: CardNumber,
                  name: CardDescription
              }))
            : [];
    }, [activeCards]);

    const isButtonDisabled = useMemo(() => {
        return (
            !agreedWithTerms ||
            !confirmActualInterest ||
            (IsAgreementNeeded && !IsAgreementConfirmed)
        );
    }, [agreedWithTerms, confirmActualInterest, IsAgreementNeeded, IsAgreementConfirmed]);

    const onSubmit = async (values: IApplicationAgreed) => {
        setSubmitting(true);
        try {
            await saveApplicationAgree(
                {
                    ...values,
                    LOAN_TYPE_ID: loanTypeId,
                    AGREED_WITH_TERMS: true,
                    ACTUAL_INTEREST: Number(loanInterest2.value),
                    SUBMIT: true
                },
                id
            );
            startPolling();
        } finally {
            setSubmitting(false);
        }
    };

    const handleSaveLoanContract = (name: string) => async (event: any) => {
        const url = getDocumentUrl(name, id);
        const { EXISTING_CARD_CODE, IS_CARD_DELIVERY, ...values } = getValues();
        if (statusID === 13) {
            await saveApplicationAgree(
                {
                    ...values,
                    LOAN_TYPE_ID: loanTypeId,
                    EXISTING_CARD_CODE: EXISTING_CARD_CODE || '',
                    IS_CARD_DELIVERY: IS_CARD_DELIVERY === 'true',
                    AGREED_WITH_TERMS: true,
                    SUBMIT: false
                },
                id
            );
            openFile(url);
        } else {
            openFile(url);
        }
    };

    const handleOpenArbitrageDoc = async () => {
        openFile(getDocumentUrl('DOC_ARBITRAGE_AGREEMENT', id));
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setRefechInterval(false);
    };

    const handleContinueRequest = () => {
        navigate('/');
    };

    useEffect(() => {
        if (applicationAgreedData) {
            reset({
                ...applicationAgreedData,
                IS_CARD_DELIVERY: applicationAgreedData?.IS_CARD_DELIVERY?.toString()
            });
        }
    }, [reset, applicationAgreedData]);

    useEffect(() => {
        if (isCardAccount) {
            setValidationContext({ isCardAccount });
        }
    }, [isCardAccount]);

    useEffect(() => {
        if (pollingTime && updatedAt) {
            const diff = differenceInMinutes(new Date(updatedAt), pollingTime);
            // is polling applicatoin more then 3 minute
            if (diff >= 3) {
                setErrorCode('timeout');
            }
        }
    }, [pollingTime, updatedAt]);

    useEffect(() => {
        if (!!errorCode || statusID === 21) {
            setRefechInterval(false);
        }
    }, [errorCode, statusID]);

    useEffect(() => {
        if (statusID !== 21) {
            setLoanIntetest2(p => ({ ...p, loading: true }));
            const getPersonalSheet = async () => {
                try {
                    const { data } = await Api.get(`/Applications/${id}/Agreed/personalSheet`);
                    if (data) {
                        setLoanIntetest2(p => ({
                            loading: false,
                            value: String(data.LOAN_INTEREST_2)
                        }));
                        setActualIntConfirmText(
                            actualInterestUserText.replace('{X}', data.LOAN_INTEREST_2)
                        );
                    }
                } finally {
                    setLoanIntetest2(p => ({ ...p, loading: false }));
                }
            };

            getPersonalSheet();
        }
    }, [id, statusID]);

    useEffect(() => {
        if (IsAgreementNeeded) {
            const { firstNameLastName, getAmount, payAmount } = agremmentValues || {};

            if (
                `${firstName} ${lastName}` === firstNameLastName &&
                Number(getAmount) === finalAmount &&
                Number(payAmount) === finalAmount
            ) {
                setIsAgreemenConfirmed(true);
            } else {
                setIsAgreemenConfirmed(false);
            }
        }
    }, [firstName, lastName, finalAmount, agremmentValues, IsAgreementNeeded]);

    return (
        <FieldSet className="application-form-wrapper" disabled={disabled}>
            <FormProvider {...methods}>
                <Form onSubmit={handleSubmit(onSubmit)} id="ContractForm">
                    <Row className="mb-3">
                        <Col sm={12}>
                            <h2 className="fieldset-title">
                                ???????????????????? ????????????{' '}
                                {disabled && (
                                    <StatusBadge status={statusState ?? ''}>
                                        {statusText}
                                    </StatusBadge>
                                )}
                            </h2>
                            <p className="form-label mb-3">
                                {hasActiveCards ? receiveLoanByExistingCard : receiveLoanByNewCard}
                            </p>
                        </Col>

                        {hasActiveCards && (
                            <Col sm={12}>
                                <Controller
                                    as={SelectField}
                                    control={control}
                                    name="EXISTING_CARD_CODE"
                                    error={errors.EXISTING_CARD_CODE}
                                    options={activeCardsOptions}
                                    loading={isActiveCardsLoading}
                                    label="???????????? ?????????? ????????????"
                                    disabled={isNewCard}
                                />
                            </Col>
                        )}
                    </Row>
                    {isCardAccount && (
                        <Row className="mb-3">
                            <Col sm={12}>
                                <Form.Group controlId="newCardControl">
                                    <Form.Check
                                        type="checkbox"
                                        ref={register}
                                        label="???????????????? ?????? ????????"
                                        id="newCardControl"
                                        name="IS_NEW_CARD"
                                        isInvalid={errors.IS_NEW_CARD}
                                    />
                                </Form.Group>
                            </Col>
                        </Row>
                    )}

                    {isCardAccount && isNewCard && (
                        <>
                            <Row className="mb-3">
                                <Col sm={12}>
                                    <Controller
                                        as={SelectField}
                                        control={control}
                                        name="CREDIT_CARD_TYPE_CODE"
                                        error={errors.CREDIT_CARD_TYPE_CODE}
                                        options={creditCardTypeOptions}
                                        loading={isCreditCardTypesLoading}
                                        label="???????????? ?????????? ????????????"
                                    />
                                </Col>
                            </Row>
                            <Row className="mb-3">
                                <Col sm={3}>
                                    <Form.Group controlId="deliverCardControl">
                                        <Form.Check
                                            type="radio"
                                            label="??????????????????"
                                            name="IS_CARD_DELIVERY"
                                            ref={register}
                                            defaultValue="true"
                                            isInvalid={errors.IS_CARD_DELIVERY}
                                        />
                                    </Form.Group>
                                </Col>
                                <Col sm={3}>
                                    <Form.Group controlId="deliverCardInBankControl">
                                        <Form.Check
                                            type="radio"
                                            label="????????????????????????"
                                            name="IS_CARD_DELIVERY"
                                            ref={register}
                                            defaultValue="false"
                                            isInvalid={errors.IS_CARD_DELIVERY}
                                        />
                                    </Form.Group>
                                </Col>
                            </Row>

                            <Row className="mb-3">
                                <Col sm={12}>
                                    <p className="mb-0">
                                        ?????????? ?????????? ?????????????????????????? ?????????????????????? ?????????????? ???????????????? ??
                                        1,000 ???? ???????? ????????????????????????:
                                    </p>
                                </Col>
                            </Row>

                            {isCartDelivery === 'true' && (
                                <Row className="mb-3">
                                    <Col sm={12}>
                                        <TextField
                                            ref={register}
                                            name="CARD_DELIVERY_ADDRESS"
                                            label="???????? ?????????? ?????????????? ????????????"
                                            error={errors.CARD_DELIVERY_ADDRESS}
                                        />
                                    </Col>
                                </Row>
                            )}

                            {isCartDelivery === 'false' && (
                                <Row className="mb-3">
                                    <Col sm={12}>
                                        <Controller
                                            as={SelectField}
                                            control={control}
                                            name="BANK_BRANCH_CODE"
                                            error={errors.BANK_BRANCH_CODE}
                                            options={bankBranchesOptions}
                                            loading={isBankBranchesLoading}
                                            label="???????? ?????? ??????????????????????, ?????????????? ?????????????????? ???? ???????????? ?????????? "
                                        />
                                    </Col>
                                </Row>
                            )}
                        </>
                    )}

                    <Row className="mb-3">
                        <Col sm={12}>
                            <Form.Group controlId="ContractTermsCheckbox">
                                <Form.Check
                                    type="checkbox"
                                    className="mb-3"
                                    ref={register}
                                    name="AGREED_WITH_TERMS"
                                    defaultValue="false"
                                    label={
                                        <div className="checkbox-label">
                                            ?????????????????? ????, ???? ?????????????????? ???? ?? ???????????????? ???? ??????????????????
                                            ??????????
                                            <Button
                                                variant="link"
                                                onClick={handleSaveLoanContract(
                                                    'DOC_INDIVIDUAL_SHEET'
                                                )}
                                            >
                                                ?????????? ???????????????????? ?????????????????? ????????????????
                                            </Button>
                                            ??
                                            <Button
                                                variant="link"
                                                onClick={handleSaveLoanContract(
                                                    'DOC_LOAN_CONTRACT'
                                                )}
                                            >
                                                ???????????????? ?????????????????????? ?????????????????? ?????????? ??????????????????????
                                            </Button>
                                        </div>
                                    }
                                />
                            </Form.Group>
                        </Col>
                        <Col sm={12}>
                            <Form.Group controlId="ArbitrageTermsCheckbox">
                                <Form.Check
                                    type="checkbox"
                                    ref={register}
                                    name="IS_ARBITRAGE_CHECKED"
                                    label={
                                        <div className="checkbox-label">
                                            ?????????????????? ????, ???? ???????????????? ????
                                            <Button variant="link" onClick={handleOpenArbitrageDoc}>
                                                ???????????????????????? ?????????????????????????? ?????????????????? ????????????????????????
                                            </Button>
                                        </div>
                                    }
                                />
                            </Form.Group>
                        </Col>
                    </Row>
                </Form>
                <Row className="mb-3">
                    <Col sm={12}>
                        {loanInterest2.loading && (
                            <div className="d-flex justify-content-center align-items-center min-h-200">
                                <Spinner animation="border" size="sm" variant="primary" />
                            </div>
                        )}
                        {loanInterest2.value && (
                            <Alert variant="info">
                                <p>
                                    ?????? ?????????? ?????????????????? ?????????? ?????????????? ?????????????????????????? ?????????????? ??{' '}
                                    <b>{loanInterest2.value}%</b>
                                </p>
                                <p>
                                    ?????????????????? ???????????????? ?? ???????????? ???????????????? ???????????? ???????? ???????????? ??????????
                                    ????????????` ?????????????????????? ?????????????? ?????????????? ?????????????????????????? ???????????
                                </p>
                                <p className="font-weight-bold">
                                    ?????????????????? ????, ???? ?????????????? ?????????????? ?????????????????????????? ???????? ?????????????? ??{' '}
                                    <span className="text-danger">{loanInterest2.value}</span> ??????????
                                </p>
                                <Row className="mb-3">
                                    <Col>
                                        <Form.Group className="mt-2 mb-0">
                                            <Form.Control
                                                rows={3}
                                                as="textarea"
                                                onPaste={(e: any) => e.preventDefault()}
                                                onChange={e => {
                                                    setConfirmActualInterest(
                                                        e.target.value
                                                            .toLowerCase()
                                                            .replaceAll('???', '.') // replacing armenian dot character with english
                                                            .replace(/\s/g, '') ===
                                                            actualIntConfirmText
                                                                .toLowerCase()
                                                                .replace(/\s/g, '')
                                                    );
                                                }}
                                            />
                                        </Form.Group>
                                    </Col>
                                </Row>
                            </Alert>
                        )}

                        {IsAgreementNeeded && (
                            <Alert variant="info" className="form-alert">
                                <p>
                                    <strong>?????????????????? ?? ?????????????? ???????????????? ??????????????</strong>
                                </p>
                                <div>
                                    ????????
                                    <Form.Control
                                        className="inline-input"
                                        as="input"
                                        onPaste={(e: any) => e.preventDefault()}
                                        style={{ width: 150 }}
                                        ref={register}
                                        placeholder={`${firstName} ${lastName}`}
                                        name="agreement.firstNameLastName"
                                    />
                                    ??????????????????
                                    <Controller
                                        name="agreement.getAmount"
                                        as={NumberFormatField}
                                        control={control}
                                        placeholder={finalAmount}
                                        setValue={setValue}
                                        className="inline-input"
                                        style={{ maxWidth: 80 }}
                                        onPaste={(e: any) => e.preventDefault()}
                                    />
                                    ?????????????? ?????????????? ???????????????????????? ???? ?????????????????? ?????????????????? ??????????
                                    <Controller
                                        name="agreement.payAmount"
                                        as={NumberFormatField}
                                        placeholder={finalAmount}
                                        className="inline-input"
                                        control={control}
                                        setValue={setValue}
                                        style={{ maxWidth: 80, marginTop: 10 }}
                                        onPaste={(e: any) => e.preventDefault()}
                                    />
                                    ???????????? ?? ?????????????????? ????, ???? ???????????????? ???????????????????????????????? ?????????????????? ??
                                    ???????????? ???????????????????? ?????????????? ???????????????? ????{' '}
                                    <b>
                                        ??????????/???????????????????? ?? ?????????????????? ?? ???? ???????????????? ????????????????????????,
                                        ???????????????? ?? ?????? ???????????? ???? ?????????????? ????????????????
                                    </b>
                                </div>
                            </Alert>
                        )}
                        <p className="mb-0">
                            ???????????? ?????????????????? ?????????????? ?????? ???????????????? ???????????? ?????????? ???????????????????? ??????????????????
                            ??????????????:
                        </p>
                    </Col>
                </Row>
                <div className="form-actions">
                    <LoadingButton
                        type="submit"
                        variant="primary"
                        form="ContractForm"
                        loading={submitting}
                        disabled={isButtonDisabled}
                    >
                        ????????????????
                    </LoadingButton>
                </div>
                <a
                    href={termsAndTariffLink}
                    className="d-block mt-4"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    ???????????????????????? ??????-?? ???????????????????????????????? ???????????????? ?????????????????? ?? ??????????????????
                </a>
                <a
                    href={rulsLink}
                    className="d-block mb-4"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    ???????????????????????? ??????-?? ???????????????? ?????????????? ???????????????????? ?????????????????? ?? ?????????????????????? ??????????????????
                </a>
            </FormProvider>
            <Modal show={showModal} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>?????????????????? ?? ??????????????</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <ContractModalContent statusId={statusID} errorCode={errorCode} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="outline-primary" as={Link} to="/">
                        ???????????????????? ?????????????? ????
                    </Button>
                    {statusID === 21 && (
                        <Button variant="primary" onClick={handleContinueRequest}>
                            ????????????????????
                        </Button>
                    )}
                </Modal.Footer>
            </Modal>
        </FieldSet>
    );
}

export default ApplicationContract;
