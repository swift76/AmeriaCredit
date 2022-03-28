import { RegAddress, CurrentAddress } from './common';

export interface IPreApplication {
    ID?: string;
    LOAN_TYPE_ID: string;
    LOAN_TYPE_STATE: string;
    INITIAL_AMOUNT: number;
    CURRENCY_CODE: string;
    FIRST_NAME: string;
    LAST_NAME: string;
    MOBILE_PHONE_1: string;
    PATRONYMIC_NAME: string;
    BIRTH_DATE: string;
    SOCIAL_CARD_NUMBER: string;
    DOCUMENT_TYPE_CODE: string;
    DOCUMENT_NUMBER: string;
    PRODUCT_CATEGORY_CODE: string;
    LOAN_TEMPLATE_CODE: string;
    ORGANIZATION_ACTIVITY_CODE: string;
    MOBILE_PHONE_AUTHORIZATION_CODE: string;
    AGREED_WITH_TERMS: boolean;
    SUBMIT: boolean;
}

export interface IApplicationMain extends RegAddress, CurrentAddress {
    ID: string;
    BIRTH_PLACE_CODE: number;
    CITIZENSHIP_CODE: number;
    COMMUNICATION_TYPE_CODE: number;
    DOC_PASSPORT: string;
    DOC_SOC_CARD: string;
    EMAIL: string;
    FINAL_AMOUNT: string;
    FIRST_NAME_EN: string;
    FIXED_PHONE: string;
    INTEREST: string;
    LAST_NAME_EN: string;
    MOBILE_PHONE: string;
    MOBILE_PHONE_2: string;
    PERIOD_TYPE_CODE: number;
    PRE_PAYMENT: string;
    PRODUCT_NUMBER: string;
    LOAN_TYPE_STATE: string;
    SUBMIT: boolean;
    Products: IProduct[];
}

export type IProduct = {
    DESCRIPTION: string;
    QUANTITY: number;
    PRICE: number;
    PRODUCT_CATEGORY_CODE?: string;
    ID: string;
};

export type IScoringResults = {
    AMOUNT: number;
    INTEREST: number;
    PREPAYMENT_AMOUNT: number;
    PREPAYMENT_INTEREST: number;
    SERVICE_AMOUNT: number;
    SERVICE_INTEREST: number;
    TEMPLATE_CODE: number | null;
    TEMPLATE_NAME: string | null;
    TERM_FROM: number;
    TERM_TO: number;
};

export type ITemplateResults = {
    AMOUNT: number;
    INTEREST: number;
    TERM_FROM: number;
    TERM_TO: number;
    TEMPLATE_CODE: string;
    TEMPLATE_NAME: string;
    SERVICE_AMOUNT: number;
    SERVICE_INTEREST: number;
    PREPAYMENT_AMOUNT: number;
    PREPAYMENT_INTEREST: number;
};

export interface IApplication {
    AGREED_WITH_TERMS: boolean;
    AMOUNT: number;
    BIRTH_DATE: string;
    CLIENT_CODE: string;
    CREATION_DATE: string;
    CURRENCY_CODE: string;
    DISPLAY_AMOUNT: string | null;
    DOCUMENT_NUMBER: string;
    DOCUMENT_TYPE_CODE: string;
    FIRST_NAME: string;
    ID: string;
    INITIAL_AMOUNT: number;
    IS_DATA_COMPLETE: boolean;
    IS_REFINANCING: boolean;
    IS_STUDENT: boolean;
    LAST_NAME: string;
    LOAN_TEMPLATE_CODE: string;
    LOAN_TYPE_AM: string | null;
    LOAN_TYPE_EN: string | null;
    LOAN_TYPE_ID: string;
    LOAN_TYPE_STATE: string;
    MANUAL_REASON: string | null;
    MOBILE_PHONE_1: string;
    MOBILE_PHONE_AUTHORIZATION_CODE: string | null;
    ONBOARDING_ID: string | null;
    ORGANIZATION_ACTIVITY_CODE: string | null;
    PARTNER_COMPANY_CODE: string | null;
    PATRONYMIC_NAME: string;
    PRODUCT_CATEGORY_CODE: string;
    REFUSAL_REASON: string | null;
    SOCIAL_CARD_NUMBER: string;
    STATUS_AM: string | null;
    STATUS_EN: string | null;
    STATUS_ID: number;
    STATUS_STATE: string;
    SUBMIT: boolean;
    UNIVERSITY_CODE: string | null;
    UNIVERSITY_FACULTY: string;
    UNIVERSITY_YEAR: string;
}
