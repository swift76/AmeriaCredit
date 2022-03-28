export type RegAddress = {
    REGISTRATION_COUNTRY_CODE: null | string;
    REGISTRATION_STATE_CODE: null | string;
    REGISTRATION_CITY_CODE: null | string;
    REGISTRATION_STREET: string;
    REGISTRATION_BUILDNUM: string;
    REGISTRATION_APARTMENT: string;
};

export type CurrentAddress = {
    CURRENT_COUNTRY_CODE: null | string;
    CURRENT_STATE_CODE: null | string;
    CURRENT_CITY_CODE: null | string;
    CURRENT_STREET: string;
    CURRENT_BUILDNUM: string;
    CURRENT_APARTMENT: string;
};
