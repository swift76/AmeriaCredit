// eslint-disable-next-line import/prefer-default-export
export const ActionTypes = {
    REMOVE: 'REMOVE',
    CANCEL: 'CANCEL',
    PRINT: 'PRINT',
    APPROACHBANK: 'APPROACHBANK'
};

export const TabsKeyEnum = {
    BASE: 'BASE',
    DETAILS: 'DETAILS',
    CONTRACT: 'CONTRACT',
    VERIFICATION: 'VERIFICATION'
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

export const WORKING_DETAILS_NAMES = {
    COMPANY_NAME: 'COMPANY_NAME',
    COMPANY_PHONE: 'COMPANY_PHONE',
    POSITION: 'POSITION',
    MONTHLY_INCOME_CODE: 'MONTHLY_INCOME_CODE',
    WORKING_EXPERIENCE_CODE: 'WORKING_EXPERIENCE_CODE'
};

export const LOAN_HINTS = {
    '01': `Սպառողական վարկերը նախատեսված են ֆիզիկական անձանց անձնական կամ ընտանեկան սպառողական ծախսերի ֆինանսավորման համար: Սպառողական վարկերը տրամադրվում են միայն ՀՀ դրամով։ <a href="https://www.ameriabank.am/Content.aspx?id=Express+Loans(retail)&page=99&itm=Retail+1.6.2&lang=33" target="_blank">Իմանալ ավելին</a>`,
    '02': `Ընթացիկ ծախսերի իրականացման նպատակով որպես կանոն քարտերի միջոցով տրամադրվող վարկեր, որոնց վճարումներն իրականացվում են ամսական մարումներով։ <a href="http://ameriabank.am/Content.aspx?id=Credit+line+on+Payment+Cards&page=99&itm=Retail+1.6.1&lang=33" target="_blank">Իմանալ ավելին</a>`,
    '03': `Կանխիկացման, վճարումների իրականացման նպատակով դեբետային քարտերի միջոցով տրամադրվող վարկեր: Քարտին կատարված վճարումներից/մուտքերից գանձվում է հաշվարկված տոկոսները, մյուս մասը վերադառնում է քարտին։ <a href="http://ameriabank.am/Content.aspx?id=Credit+line+on+Payment+Cards&page=99&itm=Retail+1.6.1&lang=33" target="_blank">Իմանալ ավելին</a>`,
    '04': `Որպես կանոն վճարումների իրականացման նպատակով կրեդիտային քարտերի միջոցով տրամադրվող վարկեր: Վարկերը տրամադրվում են արտոնյալ ժամանակահատվածով, որի ընթացքում օգտագործված գումարներն ամբողջությամբ մուտքագրելու դեպքում տոկոս չի հաշվարկվում։ <a href="https://ameriabank.am/Content.aspx?id=Credit+line+on+Payment+Cards&page=99&itm=Retail+1.6.1&lang=33" target="_blank">Իմանալ ավելին</a>`
};

export const CARD_NUMBER_FORMAT_ERROR =
    'Տվյալների անհամապատասխանություն. խնդրում ենք ճշգրտել։ Հակառակ դեպքում վարկը ստանալու համար խնդրում ենք անձը հաստատող փաստաթղթով մոտենալ «Ամերիաբանկ» ՓԲԸ-ի ցանկացած մասնաճյուղ կամ կապ հաստատել «Ամերիաբանկ» ՓԲԸ-ի  հետ (+37410) 56 11 11 հեռախոսահամարով։ «Ամերիաբանկ» ՓԲԸ-ի սպասարկման ցանցը և աշխատանքի ժամանակացույցը ներկայացված են <a href = "http://ameriabank.am/Infrastructure.aspx?&lang=33" target="_blank"><b>հետևյալ հղումով:</b></a>';
