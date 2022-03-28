export type ILoanTypes = {
    CODE: string;
    IS_CARD_ACCOUNT: boolean;
    IS_OVERDRAFT: boolean;
    NAME: string;
    STATE: string;
};

export type ILoanLimits = {
    FROM_AMOUNT: number;
    TO_AMOUNT: number;
};

export type ILoanRefinancing = {
    APPLICATION_ID?: string | null;
    CURRENCY?: string | null;
    CURRENT_BALANCE: number | null;
    DRAWDOWN_DATE?: string | null;
    INITIAL_AMOUNT?: number | null;
    INITIAL_INTEREST?: number | null;
    LOAN_CODE?: number | null;
    LOAN_TYPE?: string | null;
    MATURITY_DATE?: string | null;
    ORIGINAL_BANK_NAME?: string | null;
    ROW_ID?: number | null;
};

export type ISaveRefinancingLoan = {
    APPLICATION_ID: string;
    LOAN_CODE: string;
    ROW_ID: string;
};
