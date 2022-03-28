/* eslint-disable @typescript-eslint/no-unused-expressions */
import React, { useMemo, useCallback, useState, useEffect } from 'react';
import { Form, Row, Col, Button } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { v4 as uuidv4 } from 'uuid';
import { ISelectOption, IProduct, IApplicationMain } from 'types';
import {
    buildSelectOption,
    numberWithCommas,
    buildPeriodTypOptionsFromTemplateResult,
    calculatePrePayment
} from 'helpers/data';
import {
    useApplicationMain,
    useLoanLimits,
    useCommunicationTypes,
    useTemplateResult,
    useScoringResult,
    useApplication
} from 'hooks';
import { TextField, SelectField, NumberFormatField } from 'components/Form';
import { PersonalDetails, AddressDetails, FieldSet } from 'components/fieldset';
import Loading from 'components/Loading';
import LoadingButton from 'components/LoadingButton';
import LoadingOverlay from 'components/LoadingOverlay';
import { getRepaymentSchedule, saveApplicationMain } from 'queryies';
import { validationErrors } from 'constants/application';
import { useApplicationState } from '../Provider';
import Documents from './Documents';
import ProductDetails from './ProductDetails';

type Props = {
    disabled: boolean;
};

const BANK_CLIENT_FIELDS = [
    'FIRST_NAME_EN',
    'LAST_NAME_EN',
    'FIRST_NAME',
    'LAST_NAME',
    'PATRONYMIC_NAME',
    'BIRTH_DATE',
    'MOBILE_PHONE_2',
    'FIXED_PHONE',
    'EMAIL',
    'REGISTRATION_COUNTRY_CODE',
    'REGISTRATION_STATE_CODE',
    'REGISTRATION_CITY_CODE',
    'REGISTRATION_STREET',
    'REGISTRATION_BUILDNUM',
    'REGISTRATION_APARTMENT',
    'CURRENT_COUNTRY_CODE',
    'CURRENT_STATE_CODE',
    'CURRENT_CITY_CODE',
    'CURRENT_STREET',
    'CURRENT_BUILDNUM',
    'CURRENT_APARTMENT'
];

function ApplicationMainForm({ disabled }: Props) {
    const loanTypeId = '00';
    const [monthlyAmount, setMonthlyAmount] = useState<number | null>(null);
    const [products, setProducts] = useState<IProduct[]>([]);
    const [totalPrice, setTotalPrice] = useState(0);
    const [productError, setProductError] = useState(false);
    const [isRepaymentCalculating, setIsRepaymentCalculating] = useState(false);
    const {
        register,
        watch,
        control,
        errors,
        setValue,
        getValues,
        reset,
        handleSubmit,
        setError
    } = useFormContext();
    const { applicationId } = useApplicationState();
    const navigate = useNavigate();
    const { data: scoringResult } = useScoringResult(applicationId, {
        enabled: !!applicationId
    });
    const { data: initialApplication } = useApplication(applicationId, {
        enabled: !!applicationId
    });
    const { data: applicationMain, isLoading: isApplicationLoading } = useApplicationMain(
        applicationId,
        {
            enabled: !!applicationId
        }
    );

    const {
        data: communicationTypes,
        isLoading: isCommunicationTypesLoading
    } = useCommunicationTypes();

    const productCategoryCode = initialApplication?.PRODUCT_CATEGORY_CODE;
    const interest = scoringResult?.INTEREST;
    const amount = scoringResult?.AMOUNT;
    const initialAmount = initialApplication?.INITIAL_AMOUNT;
    const currency = initialApplication?.CURRENCY_CODE;
    const loanTemplateCode = initialApplication?.LOAN_TEMPLATE_CODE;

    const {
        data: templateResult,
        isLoading: isTemplateResultLoading
    } = useTemplateResult(productCategoryCode, { enabled: !!productCategoryCode });

    const { data: loanLimits } = useLoanLimits(loanTypeId, currency);

    const amountFileld = watch('FINAL_AMOUNT');
    const interestFileld = watch('INTEREST');
    const durationFileld = watch('PERIOD_TYPE_CODE');

    const handleAddProduct = (product: IProduct) => {
        setProducts(prev => [...prev, { ...product, PRODUCT_CATEGORY_CODE: productCategoryCode }]);
    };

    const handleDeleteProduct = (id: string) => {
        setProducts(prev => prev.filter(product => product.ID !== id));
    };

    const onSubmit = useCallback(
        async (values: IApplicationMain) => {
            let error = false;
            if (products.length === 0) {
                setProductError(true);
                return;
            }
            templateResult &&
                scoringResult &&
                loanLimits &&
                templateResult.forEach(res => {
                    if (res.TEMPLATE_CODE === loanTemplateCode) {
                        if (amountFileld > totalPrice) {
                            setError('FINAL_AMOUNT', {
                                type: 'manual',
                                message: validationErrors.LOAN_AMOUNT_GRATER_THEN_PRODUCT_PRICE
                            });
                            error = true;
                        } else if (amountFileld > scoringResult.AMOUNT) {
                            setError('FINAL_AMOUNT', {
                                type: 'manual',
                                message:
                                    validationErrors.AMOUNT_MAX_EXCEEDS_ERROR +
                                    numberWithCommas(scoringResult.AMOUNT)
                            });
                            error = true;
                        } else if (amountFileld < loanLimits.FROM_AMOUNT) {
                            setError('FINAL_AMOUNT', {
                                type: 'manual',
                                message:
                                    validationErrors.AMOUNT_MIN_EXCEEDS_ERROR +
                                    numberWithCommas(loanLimits.FROM_AMOUNT)
                            });

                            error = true;
                        }

                        let prepaymentAmount = 0;

                        if (scoringResult.PREPAYMENT_AMOUNT === 0) {
                            prepaymentAmount =
                                (totalPrice * scoringResult.PREPAYMENT_INTEREST) / 100;
                        } else {
                            prepaymentAmount = scoringResult.PREPAYMENT_AMOUNT;
                        }

                        if (prepaymentAmount > Number(getValues('PRE_PAYMENT'))) {
                            setError('PRE_PAYMENT', {
                                type: 'manual',
                                message:
                                    validationErrors.PREPAYMENT_AMOUNT_ERROR +
                                    numberWithCommas(prepaymentAmount)
                            });

                            error = true;
                        }
                    }
                });
            if (!error) {
                await saveApplicationMain({
                    ...values,
                    ID: applicationId,
                    LOAN_TYPE_STATE: 'INSTALLATION_LOAN',
                    SUBMIT: true,
                    Products: products
                });
                navigate('/');
            }
        },
        [
            amountFileld,
            applicationId,
            getValues,
            loanTemplateCode,
            loanLimits,
            navigate,
            products,
            scoringResult,
            setError,
            templateResult,
            totalPrice
        ]
    );

    const handleCalculateClick = useCallback(async () => {
        setIsRepaymentCalculating(true);
        if (currency && amountFileld && durationFileld) {
            let serviceInterest = 0;
            let serviceAmount = 0;

            templateResult &&
                templateResult.forEach(res => {
                    if (res.TEMPLATE_CODE === initialApplication?.LOAN_TEMPLATE_CODE) {
                        serviceInterest = res.SERVICE_INTEREST;
                        serviceAmount = res.SERVICE_AMOUNT;
                    }
                });

            const { MONTHLY_PAYMENT_AMOUNT } = await getRepaymentSchedule({
                interest: interestFileld,
                amount: amountFileld,
                duration: durationFileld,
                serviceInterest,
                serviceAmount
            });
            setIsRepaymentCalculating(false);
            setMonthlyAmount(MONTHLY_PAYMENT_AMOUNT);
        }
    }, [
        interestFileld,
        amountFileld,
        durationFileld,
        currency,
        initialApplication,
        templateResult
    ]);

    useEffect(() => {
        if (applicationMain) {
            const productsWithID = applicationMain?.Products.map(product => ({
                ...product,
                ID: uuidv4()
            }));

            setProducts(productsWithID);

            if (applicationMain.FINAL_AMOUNT) {
                reset(applicationMain);
            } else {
                BANK_CLIENT_FIELDS.forEach(name => {
                    setValue(name, applicationMain[name as keyof typeof applicationMain]);
                });
            }
        }
    }, [applicationMain, reset, setProducts, setValue]);

    useEffect(() => {
        let price = 0;
        products.forEach(product => {
            price += product.PRICE * product.QUANTITY;
        });

        setTotalPrice(price);
    }, [products, setTotalPrice]);

    useEffect(() => {
        setValue('INTEREST', interest, { shouldValidate: true });
    }, [setValue, interest]);

    useEffect(() => {
        const res = calculatePrePayment(totalPrice, amountFileld);
        setValue('PRE_PAYMENT', Number.isNaN(res) ? 0 : res, {
            shouldValidate: true
        });
    }, [setValue, totalPrice, amountFileld]);

    const communicationTypesOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOption(communicationTypes);
    }, [communicationTypes]);

    const periodTypeOptions = useMemo<ISelectOption[]>(() => {
        return buildPeriodTypOptionsFromTemplateResult(templateResult);
    }, [templateResult]);

    const finalAmountPlaceholder = useMemo(() => {
        if (loanLimits && scoringResult) {
            return `${numberWithCommas(loanLimits.FROM_AMOUNT)} - ${numberWithCommas(
                scoringResult.AMOUNT
            )}`;
        }
        return '';
    }, [loanLimits, scoringResult]);

    return (
        <>
            {isApplicationLoading && (
                <LoadingOverlay>
                    <Loading />
                </LoadingOverlay>
            )}
            <Form onSubmit={handleSubmit(onSubmit)} id="ApplicationMainForm">
                <FieldSet disabled={disabled}>
                    <Col sm={12}>
                        <h1 className="fieldset-title">Լրացուցիչ տվյալներ</h1>
                    </Col>
                    <ProductDetails
                        products={products}
                        onAddProduct={handleAddProduct}
                        onDeleteProduct={handleDeleteProduct}
                        totalPrice={totalPrice}
                        error={productError}
                    />
                    <Row className="mb-3">
                        <Col sm={12}>
                            <h2 className="fieldset-title">Վարկի տվյալներ </h2>
                        </Col>
                        <Col sm={6}>
                            <TextField
                                ref={register}
                                name="INTEREST"
                                label="Տոկոսադրույք"
                                error={errors.INTEREST}
                                readOnly
                            />
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={SelectField}
                                control={control}
                                name="PERIOD_TYPE_CODE"
                                label="Ժամկետ"
                                error={errors.PERIOD_TYPE_CODE}
                                loading={isTemplateResultLoading}
                                options={periodTypeOptions}
                            />
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={NumberFormatField}
                                control={control}
                                name="FINAL_AMOUNT"
                                label="Գումար"
                                error={errors.FINAL_AMOUNT}
                                setValue={setValue}
                                placeholder={finalAmountPlaceholder}
                            />
                        </Col>
                        <Col sm={6}>
                            <TextField
                                ref={register}
                                name="PRE_PAYMENT"
                                label="Կանխավճար"
                                error={errors.PRE_PAYMENT}
                                readOnly
                            />
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={SelectField}
                                control={control}
                                name="COMMUNICATION_TYPE_CODE"
                                label="Բանկի հետ հաղորդակցման եղանակ"
                                error={errors.COMMUNICATION_TYPE_CODE}
                                options={communicationTypesOptions}
                                loading={isCommunicationTypesLoading}
                            />
                        </Col>
                        <Col sm={6}>
                            <TextField
                                ref={register}
                                name="PRODUCT_NUMBER"
                                label="Վաճառքի կոդ"
                                error={errors.PRODUCT_NUMBER}
                            />
                        </Col>
                        <Col sm={6} className="mb-3">
                            <LoadingButton
                                variant="primary"
                                onClick={handleCalculateClick}
                                loading={isRepaymentCalculating}
                                disabled={
                                    durationFileld === '0' ||
                                    amountFileld === '0' ||
                                    !amountFileld ||
                                    !durationFileld
                                }
                            >
                                Հաշվարկել ամսական վճարի չափը
                            </LoadingButton>
                        </Col>
                        {!!monthlyAmount && (
                            <Col sm={12} className="mt-4">
                                <div className="alert alert-success">
                                    {`Ձեր ամսական վճարի մոտավոր չափը կազմում է ${monthlyAmount} դրամ`}
                                </div>
                            </Col>
                        )}
                    </Row>
                    <PersonalDetails nameEN citizenShip mobilePhone fixedPhone email />

                    <AddressDetails title="Գրանցման հասցե" namePrefix="REGISTRATION" />

                    <AddressDetails title="Բնակության հասցե" namePrefix="CURRENT" isSameOption />
                </FieldSet>

                <Documents disabled={disabled} />
            </Form>
        </>
    );
}

export default ApplicationMainForm;
