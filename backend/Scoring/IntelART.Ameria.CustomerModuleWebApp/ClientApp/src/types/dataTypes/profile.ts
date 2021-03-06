import { RegAddress, CurrentAddress } from './common';

export interface IProfileData extends RegAddress, CurrentAddress {
    FIRST_NAME: string;
    LAST_NAME: string;
    FIRST_NAME_EN: string;
    LAST_NAME_EN: string;
    PATRONYMIC_NAME: string;
    BIRTH_DATE: string;
    BIRTH_PLACE_CODE: null | string;
    CITIZENSHIP_CODE: null | string;
    MOBILE_PHONE: string;
    FIXED_PHONE: null | string;
    SOCIAL_CARD_NUMBER: string;
    DOCUMENT_TYPE_CODE: string;
    DOCUMENT_NUMBER: string;
    DOCUMENT_GIVEN_DATE: string;
    DOCUMENT_EXPIRY_DATE: string;
    DOCUMENT_GIVEN_BY: string;
    COMPANY_NAME: null | string;
    IS_EMAIL_VERIFIED: null | boolean;
    COMPANY_PHONE: null | string;
    ORGANIZATION_ACTIVITY_CODE: string;
    WORKING_EXPERIENCE_CODE: null | string;
    POSITION: null | string;
    MONTHLY_INCOME_CODE: null | string;
    TOTAL_EXPERIENCE_CODE: null | string;
    FAMILY_STATUS_CODE: null | string;
    CLIENT_CODE: null | string;
    ID: null | string;
    LOGIN: null | string;
    PASSWORD: null | string;
    HASH: null | string;
    EMAIL: string;
    CREATE_DATE: null | string;
    PASSWORD_EXPIRY_DATE: null | string;
    CLOSE_DATE: null | string;
    OBJECT_STATE_ID: null | string;
    OBJECT_STATE_DESCRIPTION: null | string;
    ONBOARDING_ID: string | null;
    IS_STUDENT: boolean;
}
