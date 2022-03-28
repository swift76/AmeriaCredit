/* eslint-disable func-names */
import * as yup from 'yup';
import { ILoanLimits, ILoanParameters, ILoanRefinancing } from 'types';
import { CARD_NUMBER_FORMAT_ERROR } from 'constants/application';
import { loanCode, passport } from 'validators/regexp';

const clientCodeDependValidate = (clientCode: string, schemeObj: any) => {
    if (!clientCode) {
        return schemeObj.nullable().required();
    }
    return schemeObj;
};

export const applicationBaseScheme = yup.object().shape({
    LOAN_TYPE_ID: yup.string().required(),
    CURRENCY_CODE: yup.string().required(),
    INITIAL_AMOUNT: yup
        .string()
        .required()
        .when('$loanLimits', (loanLimits: ILoanLimits, schemeObj: any) => {
            const { FROM_AMOUNT, TO_AMOUNT } = loanLimits || {};
            if (loanLimits) {
                return schemeObj.test(
                    'min-max-compatible',
                    `Գումարը պետք է լինի ${FROM_AMOUNT} - ${TO_AMOUNT} միջակայքում`,
                    (value: any) => {
                        return value >= FROM_AMOUNT && value <= TO_AMOUNT;
                    }
                );
            }
            return schemeObj;
        }),
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
    ORGANIZATION_ACTIVITY_CODE: yup.string().required(),
    UNIVERSITY_CODE: yup.string().nullable().when('$isLoanForStudent', {
        is: true,
        then: yup.string().required()
    }),
    UNIVERSITY_FACULTY: yup.string().when('$isLoanForStudent', {
        is: true,
        then: yup.string().required()
    }),
    UNIVERSITY_YEAR: yup.string().when('$isLoanForStudent', {
        is: true,
        then: yup.string().required()
    })
});

export const applicationDetailsScheme = yup.object().shape({
    INTEREST: yup.string().nullable().required(),
    PERIOD_TYPE_CODE: yup.string().nullable().required(),
    AMOUNT: yup.string().required(),
    REPAY_DAY: yup
        .string()
        .nullable()
        .when('$loanParameters', (loanParameters: ILoanParameters, schemeObj: any) => {
            const { REPAYMENT_DAY_TO, REPAYMENT_DAY_FROM } = loanParameters || {};

            if (loanParameters?.IS_REPAY_DAY_FIXED) {
                return schemeObj;
            }

            return schemeObj
                .test(
                    'min-max-compatible',
                    `Մարման օրը պետք է լինի ${REPAYMENT_DAY_FROM} - ${REPAYMENT_DAY_TO} միջակայքում`,
                    (value: any) => {
                        return value >= REPAYMENT_DAY_FROM && value <= REPAYMENT_DAY_TO;
                    }
                )
                .required();
        }),
    FIRST_NAME_EN: yup.string().required(),
    LAST_NAME_EN: yup.string().required(),
    BIRTH_PLACE_CODE: yup.string().nullable().required(),
    CITIZENSHIP_CODE: yup.string().nullable().required(),
    MOBILE_PHONE: yup.string().required(),
    FIXED_PHONE: yup.string().nullable(),
    EMAIL: yup.string().required(),
    IS_REFINANCING: yup.string(),
    REGISTRATION_COUNTRY_CODE: yup.string().nullable().required(),
    REGISTRATION_STATE_CODE: yup.string().nullable().required(),
    REGISTRATION_CITY_CODE: yup.string().nullable().required(),
    REGISTRATION_STREET: yup.string().required(),
    REGISTRATION_BUILDNUM: yup.string().required(),
    CURRENT_COUNTRY_CODE: yup.string().nullable().required(),
    CURRENT_STATE_CODE: yup.string().nullable().required(),
    CURRENT_CITY_CODE: yup.string().nullable().required(),
    CURRENT_STREET: yup.string().nullable().required(),
    CURRENT_BUILDNUM: yup.string().nullable().required(),
    FAMILY_STATUS_CODE: yup.string().nullable().required(),
    COMPANY_NAME: yup.string().when('$clientCode', clientCodeDependValidate),
    COMPANY_PHONE: yup.string().when('$clientCode', clientCodeDependValidate),
    POSITION: yup.string().when('$clientCode', clientCodeDependValidate),
    MONTHLY_INCOME_CODE: yup.string().when('$clientCode', clientCodeDependValidate),
    WORKING_EXPERIENCE_CODE: yup.string().when('$clientCode', clientCodeDependValidate),
    DOC_PASSPORT: yup.mixed().when('$clientCode', clientCodeDependValidate),
    DOC_SOC_CARD: yup.mixed().when('$clientCode', clientCodeDependValidate),
    LOAN_CODES: yup.mixed().when('$isRefinancing', {
        is: true,
        then: yup
            .array()
            .of(
                yup.object().shape({
                    LOAN_CODE: yup.string().test({
                        message: 'Կոդը պետք է համապատասախանի ձևաչափին',
                        test: value => !value.length || loanCode.test(value)
                    })
                })
            )
            .required()
    })
});

export const applicationVerificationScheme = yup.object().shape({
    CARD_NUMBER: yup
        .string()
        .required()
        .test('len', CARD_NUMBER_FORMAT_ERROR, val => val?.length === 16),
    EXPIRY_MONTH: yup.string().required(),
    EXPIRY_YEAR: yup.string().required()
});

export const applicationContractScheme = yup.object().shape({
    EXISTING_CARD_CODE: yup.string().nullable().when('IS_NEW_CARD', {
        is: false,
        then: yup.string().required()
    }),
    CREDIT_CARD_TYPE_CODE: yup.string().when('IS_NEW_CARD', {
        is: true,
        then: yup.string().required()
    }),
    IS_CARD_DELIVERY: yup.string().when('IS_NEW_CARD', {
        is: true,
        then: yup.string().required()
    }),
    CARD_DELIVERY_ADDRESS: yup
        .string()
        .nullable()
        .when(['IS_NEW_CARD', 'IS_CARD_DELIVERY'], {
            is: (newCart, deliver) => newCart && deliver === 'true',
            then: yup.string().required()
        }),
    BANK_BRANCH_CODE: yup.string().nullable().when(['IS_CARD_DELIVERY'], {
        is: 'false',
        then: yup.string().required()
    })
});

export const validateRefinancing = (
    refinancingData?: ILoanRefinancing[],
    loanCodes?: ILoanRefinancing[],
    fromAmout?: number,
    approvedAmount?: number
) => {
    if (refinancingData && loanCodes && fromAmout && approvedAmount) {
        let total = 0;

        loanCodes.forEach((loan: any, index: number) => {
            if (loan.LOAN_CODE) {
                const loanData = refinancingData[index];

                if (loanData.CURRENT_BALANCE) {
                    total += loanData.CURRENT_BALANCE;
                }
            }
        });
        return total > fromAmout && total < approvedAmount;
    }

    return false;
};
