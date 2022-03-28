// eslint-disable-next-line import/prefer-default-export
export const ActionTypes = {
    REMOVE: 'REMOVE',
    CANCEL: 'CANCEL',
    PRINT: 'PRINT',
    APPROACHBANK: 'APPROACHBANK'
};

export const TabsKeyEnum = {
    PRE_APPLICATION: 'PRE_APPLICATION',
    APPLICATION_MAIN: 'APPLICATION_MAIN'
} as const;

export const APPLICATION_STATUS_ENUM = {
    NEW: 'NEW',
    PENDING_PRE_APPROVAL: 'PENDING_PRE_APPROVAL',
    PRE_APPROVAL_SUCCESS: 'PRE_APPROVAL_SUCCESS',
    PRE_APPROVAL_FAIL: 'PRE_APPROVAL_FAIL',
    PRE_APPROVAL_REVIEW: 'PRE_APPROVAL_REVIEW',
    PENDING_APPROVAL: 'PENDING_APPROVAL',
    APPROVAL_REVIEW: 'APPROVAL_REVIEW',
    APPROVAL_SUCCESS: 'APPROVAL_SUCCESS',
    APPROVAL_FAIL: 'APPROVAL_FAIL',
    AGREED: 'AGREED',
    CANCELLED: 'CANCELLED',
    PHONE_VERIFICATION_PENDING: 'PHONE_VERIFICATION_PENDING',
    DELIVERING: 'DELIVERING',
    COMPLETED: 'COMPLETED',
    EXPIRED: 'EXPIRED'
};

export const pendingStatuses = [
    APPLICATION_STATUS_ENUM.NEW,
    APPLICATION_STATUS_ENUM.PENDING_PRE_APPROVAL,
    APPLICATION_STATUS_ENUM.PENDING_APPROVAL,
    APPLICATION_STATUS_ENUM.PHONE_VERIFICATION_PENDING,
    APPLICATION_STATUS_ENUM.APPROVAL_REVIEW
];

export const rejectedStatuses = [
    APPLICATION_STATUS_ENUM.EXPIRED,
    APPLICATION_STATUS_ENUM.PRE_APPROVAL_FAIL,
    APPLICATION_STATUS_ENUM.CANCELLED
];

export const approvedStatuses = [
    APPLICATION_STATUS_ENUM.PRE_APPROVAL_SUCCESS,
    APPLICATION_STATUS_ENUM.APPROVAL_SUCCESS,
    APPLICATION_STATUS_ENUM.AGREED,
    APPLICATION_STATUS_ENUM.DELIVERING,
    APPLICATION_STATUS_ENUM.COMPLETED
];
export const ADDRESS_DETAILS_NAMES = [
    'COUNTRY_CODE',
    'STATE_CODE',
    'CITY_CODE',
    'STREET',
    'BUILDNUM',
    'APARTMENT'
];

export const USER_ROLES = {
    MANAGER: 3,
    USER: 2
};

export const validationErrors = {
    DEFAULT_DATE_FORMAT: 'dd/mm/yyyy',
    DEFAULT_COLUMN_VALUE: '',
    REQUIRED_FIELD_ERROR: 'Պարտադիր լրացման դաշտ',
    EMAIL_FORMAT_ERROR: 'Էլ․ հասցեի ֆորմատը սխալ է',
    DATE_FORMAT_ERROR: 'Ամսաթվի ֆորմատը սխալ է',
    DATE_YEAR_FORMAT_ERROR: 'Տարեթիվը պետք է ունենա 4 նիշ',
    DATE_BIRTHDATE_FORMAT_ERROR: 'Ծննդյան ամսաթիվը չի կարող գերազանցել ընթացիկ ամսաթիվը',
    DATE_DOCUMENT_GIVEN_FORMAT_ERROR:
        'Փաստաթղթի տրման ամսաթիվը չի կարող գերազանցել ընթացիկ ամսաթիվը',
    DATE_DOCUMENT_EXPIRED_FORMAT_ERROR: 'Փաստաթղթի վավերականության ժամկետը լրացել է',
    NUMBER_FORMAT_ERROR: 'Թվի ֆորմատը սխալ է',
    PHONE_FORMAT_ERROR: 'Հեռախոսի ֆորմատը սխալ է։ Պահանջվում է 8 թիվ։',
    PASSWORDS_DONT_MATCH: 'Գաղտնաբառերը չեն համընկնում ',
    EMPTY_REQUEST_RESPONSE: 'Ձեր կատարած հարցմանը բավարարող տվյալ չկա',
    WRONG_DOCUMENT_NUMBER: 'Փաստաթղթի համարը սխալ է',
    AMOUNT_MAX_EXCEEDS_ERROR: 'Թույլատրելի առավելագույն գումարը կազմում է ',
    AMOUNT_MIN_EXCEEDS_ERROR: 'Թույլատրելի նվազագույն գումարը կազմում է ',
    REPAY_DAY_ERROR: 'Թույլատրելի մարման օրերն են՝ ',
    LOAN_AMOUNT_GRATER_THEN_PRODUCT_PRICE: 'Ապառիկի գումարը չի կարող գերազանցել ապրանքի գումարը',
    PREPAYMENT_AMOUNT_ERROR: 'Կանխավճարի նվազագույն գումարը կազմում է '
};
