import { ILoanRefinancing } from 'types';
import { RegAddress, CurrentAddress } from './common';

export interface IApplication {
    ID: string | null;
    INITIAL_AMOUNT: number;
    CURRENCY_CODE: string | null;
    FIRST_NAME: string | null;
    LAST_NAME: string | null;
    PATRONYMIC_NAME: string | null;
    BIRTH_DATE: string | null;
    SOCIAL_CARD_NUMBER: string | null;
    DOCUMENT_TYPE_CODE: string | null;
    DOCUMENT_NUMBER: string | null;
    ORGANIZATION_ACTIVITY_CODE: string | null;
    MOBILE_PHONE_AUTHORIZATION_CODE: string | null;
    AGREED_WITH_TERMS: boolean;
    CLIENT_CODE: string | null;
    IS_DATA_COMPLETE: boolean;
    SUBMIT: boolean;
    REFUSAL_REASON: string | null;
    MANUAL_REASON: string | null;
    IS_REFINANCING: boolean;
    CREATION_DATE: string | null;
    STATUS_ID: number;
    STATUS_AM: string | null;
    STATUS_EN: string | null;
    STATUS_STATE: string | null;
    AMOUNT: number;
    LOAN_TYPE_ID: string | null;
    LOAN_TYPE_AM: string | null;
    LOAN_TYPE_EN: string | null;
    LOAN_TYPE_STATE: string | null;
    DISPLAY_AMOUNT: string | null;
    MOBILE_PHONE_1: string | null;
    LOAN_TEMPLATE_CODE: number | null;
    PRODUCT_CATEGORY_CODE: string | null;

    UNIVERSITY_CODE?: string;
    UNIVERSITY_FACULTY?: string;
    UNIVERSITY_YEAR?: string;
}

export type IApplicationProduct = {
    PRODUCT_CATEGORY_CODE: string;
    PRICE: number;
    QUANTITY: number;
    DESCRIPTION: string;
};

export interface IApplicationMain extends RegAddress, CurrentAddress {
    ID: string;
    SOURCE_ID: string;
    REPAY_DAY: string;
    AMOUNT: string;
    FINAL_AMOUNT: string;
    INTEREST: string;
    FIRST_NAME_EN: string;
    LAST_NAME_EN: string;
    MOBILE_PHONE: string;
    MOBILE_PHONE_1: string | null;
    MOBILE_PHONE_2: string;
    FIXED_PHONE: string | null;
    EMAIL: string;
    SHOP_CODE: string;
    PRODUCT_NUMBER: string;
    GOODS_RECEIVING_CODE: string;
    GOODS_DELIVERY_ADDRESS: string;
    Products: IApplicationProduct[];
    PERIOD_TYPE_CODE: string;
    BIRTH_PLACE_CODE: null | string;
    CITIZENSHIP_CODE: null | string;
    COMMUNICATION_TYPE_CODE: string;
    COMPANY_NAME: string;
    COMPANY_PHONE: string;
    POSITION: string;
    MONTHLY_INCOME_CODE: string;
    WORKING_EXPERIENCE_CODE: string;
    FAMILY_STATUS_CODE: string;
    OVERDRAFT_TEMPLATE_CODE: number | null;
    LOAN_TEMPLATE_CODE: number | null;
    AGREED_WITH_TERMS: boolean;
    SUBMIT: boolean;
    LOAN_TYPE_STATE: string | null;

    LOAN_CODES?: ILoanRefinancing[];
    DOC_PASSPORT?: boolean;
    DOC_SOC_CARD?: boolean;
}

export type IScoringResults = {
    AMOUNT: number;
    INTEREST: number;
    TERM_FROM: string;
    TERM_TO: string;
    TEMPLATE_CODE: number;
    TEMPLATE_NAME: string;
    SERVICE_AMOUNT: number;
    SERVICE_INTEREST: number;
    PREPAYMENT_AMOUNT: number;
    PREPAYMENT_INTEREST: number;
};

export type ILoanParameters = {
    IS_CARD_ACCOUNT: boolean;
    IS_OVERDRAFT: boolean;
    IS_REPAY_DAY_FIXED: boolean;
    IS_REPAY_NEXT_MONTH: boolean;
    IS_REPAY_START_DAY: boolean;
    REPAYMENT_DAY_FROM: number;
    REPAYMENT_DAY_TO: number;
    REPAY_TRANSITION_DAY: number;
};

export type IApplicationVerification = {
    CARD_NUMBER: number;
    EXPIRY_MONTH: number;
    EXPIRY_YEAR: number;
};

export type ICard = {
    ID?: string;
    cardNumber: number;
    expiryDate: string | undefined;
};

export type IActiveCards = {
    CardNumber: string;
    CardDescription: string;
};

export type IApplicationAgreed = {
    IS_NEW_CARD?: boolean;
    ACTUAL_INTEREST?: number;
    EXISTING_CARD_CODE?: string;
    CREDIT_CARD_TYPE_CODE?: string;
    IS_CARD_DELIVERY?: string | boolean;
    IsAgreementNeeded?: boolean;
    CARD_DELIVERY_ADDRESS?: string;
    FINAL_AMOUNT?: number;
    BANK_BRANCH_CODE?: string;
    AGREED_WITH_TERMS?: boolean;
    LOAN_TYPE_ID?: string | null;
    SUBMIT: boolean;
};
