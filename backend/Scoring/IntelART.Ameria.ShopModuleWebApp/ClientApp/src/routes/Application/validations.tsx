/* eslint-disable func-names */
import * as yup from 'yup';
import { ILoanLimits } from 'types';
import { passport } from 'validators/regexp';
import { numberWithCommas } from 'helpers/data';

export const preApplicationScheme = yup.object().shape({
    INITIAL_AMOUNT: yup
        .string()
        .required()
        .when('$loanLimits', (loanLimits: ILoanLimits, schemeObj: any) => {
            const { FROM_AMOUNT, TO_AMOUNT } = loanLimits || {};
            if (loanLimits) {
                return schemeObj.test(
                    'min-max-compatible',
                    `Գումարը պետք է լինի ${numberWithCommas(FROM_AMOUNT)} - ${numberWithCommas(
                        TO_AMOUNT
                    )} միջակայքում`,
                    (value: any) => {
                        return value >= FROM_AMOUNT && value <= TO_AMOUNT;
                    }
                );
            }
            return schemeObj;
        }),
    CURRENCY_CODE: yup.string().required(),
    FIRST_NAME: yup.string().required(),
    LAST_NAME: yup.string().required(),
    PATRONYMIC_NAME: yup.string().required(),
    BIRTH_DATE: yup.string().required(),
    SOCIAL_CARD_NUMBER: yup.string().required(),
    DOCUMENT_TYPE_CODE: yup.string().required(),
    DOCUMENT_NUMBER: yup
        .string()
        .required()
        .when('DOCUMENT_TYPE_CODE', {
            is: val => ['1', '3'].includes(val),
            then: yup.string().matches(passport, { message: 'Փաստաթղթի համարը սխալ է' })
        }),
    MOBILE_PHONE_1: yup.string().required(),
    PRODUCT_CATEGORY_CODE: yup.string().required(),
    LOAN_TEMPLATE_CODE: yup.string().required(),
    MOBILE_PHONE_AUTHORIZATION_CODE: yup.string().nullable().required(),
    DOC_SCORING_REQUEST_AGREEMENT_SIGNED: yup.mixed().nullable().required()
});

export const applicationMainScheme = yup.object().shape({
    BIRTH_PLACE_CODE: yup.string().required(),
    CITIZENSHIP_CODE: yup.string().required(),
    COMMUNICATION_TYPE_CODE: yup.number().required(),
    CURRENT_APARTMENT: yup.string(),
    CURRENT_BUILDNUM: yup.string().required(),
    CURRENT_CITY_CODE: yup.string().required(),
    CURRENT_COUNTRY_CODE: yup.string().required(),
    CURRENT_STATE_CODE: yup.string().required(),
    CURRENT_STREET: yup.string().required(),
    EMAIL: yup.string().required(),
    FINAL_AMOUNT: yup.string().required(),
    FIRST_NAME_EN: yup.string().required(),
    FIXED_PHONE: yup.string(),
    INTEREST: yup.string(),
    LAST_NAME_EN: yup.string().required(),
    MOBILE_PHONE_2: yup.string(),
    PERIOD_TYPE_CODE: yup.number().required(),
    PRE_PAYMENT: yup.string().required(),
    PRODUCT_NUMBER: yup.string(),
    REGISTRATION_APARTMENT: yup.string(),
    REGISTRATION_BUILDNUM: yup.string().required(),
    REGISTRATION_CITY_CODE: yup.string().required(),
    REGISTRATION_COUNTRY_CODE: yup.string().required(),
    REGISTRATION_STATE_CODE: yup.string().required(),
    REGISTRATION_STREET: yup.string().required(),
    DOC_PASSPORT: yup.mixed().nullable().required(),
    DOC_SOC_CARD: yup.mixed().nullable().required()
});
