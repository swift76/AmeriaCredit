import {
    IOnboardingData,
    DirectoryEntity,
    ISelectOption,
    ILoanLimits,
    IProfileData,
    IScoringResults,
    ILoanTypes
} from 'types';
import { APPLICATION_STATUS_ENUM as statuses, TabsKeyEnum } from 'constants/application';

export function numberWithCommas(number: number): string {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

export const buildSelectOption = (data: DirectoryEntity[] | undefined = []): ISelectOption[] => {
    return data.map(({ CODE: value, NAME: name }) => ({ value, name })) || [];
};

export function getAmountLimitPlaceholder(loanLimits: ILoanLimits) {
    const { FROM_AMOUNT, TO_AMOUNT } = loanLimits;
    return `${numberWithCommas(FROM_AMOUNT)} - ${numberWithCommas(TO_AMOUNT)}`;
}

export function getIdentityFromProfile(profileData: IProfileData) {
    const {
        SOCIAL_CARD_NUMBER,
        DOCUMENT_TYPE_CODE,
        DOCUMENT_NUMBER,
        DOCUMENT_GIVEN_DATE,
        DOCUMENT_EXPIRY_DATE,
        DOCUMENT_GIVEN_BY
    } = profileData;

    return {
        SOCIAL_CARD_NUMBER,
        DOCUMENT_TYPE_CODE,
        DOCUMENT_NUMBER,
        DOCUMENT_GIVEN_DATE,
        DOCUMENT_EXPIRY_DATE,
        DOCUMENT_GIVEN_BY
    };
}

export function getAddressesFromProfile(profileData: IProfileData) {
    return {
        CURRENT_APARTMENT: profileData.CURRENT_APARTMENT,
        CURRENT_BUILDNUM: profileData.CURRENT_BUILDNUM,
        CURRENT_CITY_CODE: profileData.CURRENT_CITY_CODE,
        CURRENT_COUNTRY_CODE: profileData.CURRENT_COUNTRY_CODE,
        CURRENT_STATE_CODE: profileData.CURRENT_STATE_CODE,
        CURRENT_STREET: profileData.CURRENT_STREET,

        REGISTRATION_APARTMENT: profileData.CURRENT_APARTMENT,
        REGISTRATION_BUILDNUM: profileData.CURRENT_BUILDNUM,
        REGISTRATION_CITY_CODE: profileData.CURRENT_CITY_CODE,
        REGISTRATION_COUNTRY_CODE: profileData.CURRENT_COUNTRY_CODE,
        REGISTRATION_STATE_CODE: profileData.CURRENT_STATE_CODE,
        REGISTRATION_STREET: profileData.CURRENT_STREET
    };
}

export function getTabAccessByStatus(status: string, name: string) {
    switch (name) {
        case TabsKeyEnum.BASE:
            return {
                isTabEnabled: status === statuses.NEW,
                isTabAccessible: true
            };
        case TabsKeyEnum.DETAILS:
            return {
                isTabEnabled: status === statuses.PRE_APPROVAL_SUCCESS,
                isTabAccessible: [
                    statuses.PRE_APPROVAL_SUCCESS,
                    statuses.APPROVAL_REVIEW,
                    statuses.PENDING_APPROVAL,
                    statuses.APPROVAL_SUCCESS,
                    statuses.AGREED,
                    statuses.COMPLETED,
                    statuses.PHONE_VERIFICATION_PENDING
                ].includes(status)
            };
        case TabsKeyEnum.VERIFICATION:
            return {
                isTabEnabled: status === statuses.PENDING_APPROVAL,
                isTabAccessible: [
                    statuses.PENDING_APPROVAL,
                    statuses.APPROVAL_SUCCESS,
                    statuses.PHONE_VERIFICATION_PENDING,
                    statuses.AGREED
                ].includes(status)
            };
        case TabsKeyEnum.CONTRACT:
            return {
                isTabEnabled: status === statuses.APPROVAL_SUCCESS,
                isTabAccessible: [
                    statuses.APPROVAL_SUCCESS,
                    statuses.AGREED,
                    statuses.COMPLETED
                ].includes(status)
            };

        default:
            return {
                isTabEnabled: false,
                isTabAccessible: false
            };
    }
}

export function resolveIsOverdraft(loanTypes: ILoanTypes[] = [], loanTypeId: string) {
    let isOverdraft = false;

    // eslint-disable-next-line no-plusplus
    for (let index = 0; index < loanTypes.length; index++) {
        const element = loanTypes[index];
        if (element.CODE === loanTypeId) {
            isOverdraft = element.IS_OVERDRAFT;
            break;
        }
    }

    return isOverdraft;
}

export function getRepayDayValue(
    loanTypes: ILoanTypes[] = [],
    loanTypeId: string,
    isRepayStartDay: boolean
) {
    if (isRepayStartDay) {
        return '1';
    }
    return resolveIsOverdraft(loanTypes, loanTypeId) ? '28' : '1';
}

export function getDurationOptions(
    interest: IScoringResults,
    loanTypes: ILoanTypes[],
    loanTypeId: string
) {
    const options = [];
    let start = interest.TERM_FROM;
    while (start <= interest.TERM_TO) {
        options.push({ value: start.toString(), name: `${start} ամիս` });
        start += 6;
    }

    if (resolveIsOverdraft(loanTypes, loanTypeId)) {
        options.push({ value: '0', name: 'Անժամկետ' });
    }

    return options;
}

export function getUserInfoFromOnBoard(onBoardData: IOnboardingData) {
    return {
        FIRST_NAME_EN: onBoardData.first_name_eng,
        LAST_NAME_EN: onBoardData.last_name_eng,
        FIRST_NAME: onBoardData.first_name_arm,
        LAST_NAME: onBoardData.last_name_arm,
        PATRONYMIC_NAME: onBoardData.middle_name_arm,
        BIRTH_DATE: onBoardData.birth_date,
        MOBILE_PHONE: onBoardData.mobile_number,
        EMAIL: onBoardData.email,
        SOCIAL_CARD_NUMBER: onBoardData.soccard_number,
        DOCUMENT_TYPE_CODE: onBoardData.document_type_id.toString(),
        DOCUMENT_NUMBER: onBoardData.document_number,
        DOCUMENT_GIVEN_DATE: onBoardData.document_issue_date,
        DOCUMENT_GIVEN_BY: onBoardData.document_issuer,
        IS_STUDENT: onBoardData.is_student
    };
}

export function getDocumentUrl(code: string, id: string) {
    return `/api/loan/Applications/${id}/Documents/${code}`;
}

export function openFile(url: string) {
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('target', '_blank');
    document.body.appendChild(link);
    link.click();

    setTimeout(() => {
        document.body.removeChild(link);
    }, 100);
}
