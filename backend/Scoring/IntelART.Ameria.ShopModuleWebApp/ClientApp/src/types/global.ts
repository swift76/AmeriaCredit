export type ValuesOf<T extends any[]> = T[number];

export type DirectoryEntity = {
    CODE: string;
    NAME: string;
};

export type ISelectOption = {
    value: string;
    name: string;
};

export type DDLDataState = {
    fetching: boolean;
    fetched: boolean;
    data: DirectoryEntity[];
};

export type TabsKeysEnumTypes = 'PRE_APPLICATION' | 'APPLICATION_MAIN';
