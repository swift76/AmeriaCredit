export interface IApplications {
    ID: string;
    CREATION_DATE: string;
    STATUS_ID: number;
    STATUS_AM: string;
    STATUS_EN: string;
    STATUS_STATE: string;
    AMOUNT: number;
    LOAN_TYPE_ID: string;
    LOAN_TYPE_AM: string;
    LOAN_TYPE_EN: string;
    LOAN_TYPE_STATE: string;
    DISPLAY_AMOUNT: string;
    MOBILE_PHONE_1: string | null;
    LOAN_TEMPLATE_CODE: string | null;
    PRODUCT_CATEGORY_CODE: string | null;
    ONBOARDING_ID: string | null;
}
