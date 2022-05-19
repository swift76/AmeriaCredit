import React, { useEffect, useMemo, useState } from 'react';
import { Form, Row, Col, Alert } from 'react-bootstrap';
import { useFormContext, Controller } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { ISelectOption, IPreApplication } from 'types';
import {
    buildSelectOption,
    getAmountLimitPlaceholder,
    buildSelectOptionFromTemplateResult
} from 'helpers/data';
import { useCurrencies, useLoanLimits, useProductCategories, useTemplateResult } from 'hooks';
import { saveApplication, mobilePhoneAuthorization } from 'queryies';
import { SelectField, NumberFormatField, PhoneNumberInput, RestrictedInput } from 'components/Form';
import { PersonalDetails, IdentityDocuments, FieldSet } from 'components/fieldset';
import LoadingButton from 'components/LoadingButton';
import Loading from 'components/Loading';
import LoadingOverlay from 'components/LoadingOverlay';
import AgreementDocuments from './AgreementDocuments';
import { useApplicationState, useApplicationDispatch } from '../Provider';

type Props = {
    disabled: boolean;
    loading: boolean;
};

function PreApplicationForm({ disabled, loading }: Props) {
    const [error, setError] = useState({ isError: false, message: '', errorCode: '' });
    const { handleSubmit, watch, control, errors, setValue, getValues, trigger } = useFormContext();
    const navigate = useNavigate();
    const { applicationId } = useApplicationState();
    const { setApplicationId } = useApplicationDispatch();
    const loanTypeId = '00';

    const currencyCode = watch(`CURRENCY_CODE`);
    const productCategoryCode = watch(`PRODUCT_CATEGORY_CODE`);

    const { data: currenciesData, isLoading: isCurrenciesLoading } = useCurrencies(loanTypeId);
    const { data: loanLimits } = useLoanLimits(loanTypeId, currencyCode);
    const {
        data: productCategoriesData,
        isLoading: isProductCategoriesLoading
    } = useProductCategories();
    const {
        data: templateResultData,
        isLoading: isTemplateResultLoading
    } = useTemplateResult(productCategoryCode, { enabled: !!productCategoryCode });

    const onSubmit = async (values: IPreApplication) => {
        try {
            await saveApplication({
                ...values,
                ID: applicationId,
                SUBMIT: true,
                ORGANIZATION_ACTIVITY_CODE: '',
                LOAN_TYPE_ID: loanTypeId,
                AGREED_WITH_TERMS: true
            });

            navigate('/');
        } catch (err) {
            setError({ isError: true, message: err.Message, errorCode: err.ErrorCode });
        }
    };

    const handleSave = async () => {
        const formData = getValues();

        const { data } = await saveApplication({
            ...formData,
            LOAN_TYPE_ID: loanTypeId,
            SUBMIT: false,
            ORGANIZATION_ACTIVITY_CODE: ''
        } as IPreApplication);

        setApplicationId(data);
        await mobilePhoneAuthorization(data);
    };

    const handleMobilePhoneAuthorization = async () => {
        const result = await trigger(
            Object.keys(getValues()).filter(
                item =>
                    item !== 'MOBILE_PHONE_AUTHORIZATION_CODE' &&
                    item !== 'DOC_SCORING_REQUEST_AGREEMENT_SIGNED'
                // item !== 'PRODUCT_CATEGORY_CODE' &&
                // item !== 'LOAN_TEMPLATE_CODE'
            )
        );

        if (result) {
            try {
                if (applicationId) {
                    await mobilePhoneAuthorization(applicationId);
                } else {
                    handleSave();
                }
            } catch (err) {
                setError({ isError: true, message: err.Message, errorCode: err.ErrorCode });
            }
        }
    };

    // buildSelectOption
    const productCategoriesOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOption(productCategoriesData);
    }, [productCategoriesData]);

    const currenciesOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOption(currenciesData);
    }, [currenciesData]);

    const templateResultOptions = useMemo<ISelectOption[]>(() => {
        return buildSelectOptionFromTemplateResult(templateResultData);
    }, [templateResultData]);

    const amountPlaceholder = useMemo(() => {
        if (loanLimits) {
            return getAmountLimitPlaceholder(loanLimits);
        }
        return '';
    }, [loanLimits]);

    useEffect(() => {
        if (currenciesOptions.length !== 0 && !currencyCode) {
            setValue('CURRENCY_CODE', currenciesOptions[0].value, {
                shouldValidate: true
            });
        }
    }, [currenciesOptions, setValue, currencyCode]);

    useEffect(() => {
        if (templateResultOptions.length !== 0) {
            setValue('LOAN_TEMPLATE_CODE', templateResultOptions[0].value, {
                shouldValidate: true
            });
        }
    }, [templateResultOptions, setValue]);

    return (
        <>
            {loading && (
                <LoadingOverlay>
                    <Loading />
                </LoadingOverlay>
            )}
            <Form onSubmit={handleSubmit(onSubmit)} id="PreApplicationForm">
                <FieldSet disabled={disabled}>
                    <Col sm={12}>
                        <h1 className="fieldset-title">Հայտ</h1>
                    </Col>
                    <Row className="mb-3">
                        <Col sm={12}>
                            <h2 className="fieldset-title">Վարկի տվյալներ</h2>
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={NumberFormatField}
                                control={control}
                                name="INITIAL_AMOUNT"
                                label="Գումար *"
                                error={errors.INITIAL_AMOUNT}
                                setValue={setValue}
                                placeholder={amountPlaceholder}
                            />
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={SelectField}
                                control={control}
                                name="CURRENCY_CODE"
                                label="Արժույթ *"
                                error={errors.CURRENCY_CODE}
                                loading={isCurrenciesLoading}
                                options={currenciesOptions}
                            />
                        </Col>
                    </Row>
                    <PersonalDetails nameAM patronicName birthDate />
                    <IdentityDocuments />
                    <Row className="mb-3">
                        <Col sm={12}>
                            <h2 className="fieldset-title">Բջջային հեռախոս</h2>
                        </Col>
                        <Col sm={6}>
                            <PhoneNumberInput
                                name="MOBILE_PHONE_1"
                                control={control}
                                error={errors.MOBILE_PHONE_1}
                                label={false}
                            />
                        </Col>
                        <Col sm={6}>
                            <LoadingButton
                                variant="info"
                                loading={false}
                                onClick={handleMobilePhoneAuthorization}
                                disabled={error.errorCode === 'ERR-0008'}
                            >
                                Ուղարկել նույնականացման կոդը
                            </LoadingButton>
                        </Col>
                        {error.isError && error.errorCode === 'ERR-0008' && (
                            <Col sm={12}>
                                <Alert variant="danger">{error.message}</Alert>
                            </Col>
                        )}
                        <Col sm={6}>
                            <Controller
                                as={RestrictedInput}
                                control={control}
                                name="MOBILE_PHONE_AUTHORIZATION_CODE"
                                error={errors.MOBILE_PHONE_AUTHORIZATION_CODE}
                                label="Նույնականացման կոդ"
                                type="number"
                                maxLength={4}
                            />
                        </Col>
                    </Row>
                    <Row className="mb-3">
                        <Col sm={12}>
                            <h2 className="fieldset-title">Ապրանքի տվյալներ</h2>
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={SelectField}
                                control={control}
                                name="PRODUCT_CATEGORY_CODE"
                                label="Ապրանքի տեսակը"
                                error={errors.PRODUCT_CATEGORY_CODE}
                                options={productCategoriesOptions}
                                loading={isProductCategoriesLoading}
                            />
                        </Col>
                        <Col sm={6}>
                            <Controller
                                as={SelectField}
                                control={control}
                                name="LOAN_TEMPLATE_CODE"
                                label="Ակցիա"
                                error={errors.LOAN_TEMPLATE_CODE}
                                options={templateResultOptions}
                                loading={isTemplateResultLoading}
                            />
                        </Col>
                    </Row>
                </FieldSet>
                <AgreementDocuments disabled={disabled} />
            </Form>
            {error.isError && error.errorCode !== 'ERR-0008' && (
                <Alert variant="danger">{error.message}</Alert>
            )}
        </>
    );
}

export default PreApplicationForm;
