import { IApplications } from 'types';

interface IConfig {
    header: string;
    accessor: keyof IApplications;
}

export const columnsConfig: IConfig[] = [
    {
        header: 'Վարկատեսակ',
        accessor: 'LOAN_TYPE_AM'
    },
    {
        header: 'Դիմումի ամսաթիվ',
        accessor: 'CREATION_DATE'
    },
    {
        header: 'Հայտի գումար',
        accessor: 'DISPLAY_AMOUNT'
    },
    {
        header: 'Կարգավիճակ',
        accessor: 'STATUS_AM'
    },
    {
        header: 'Գործողություն',
        accessor: 'ID'
    }
];
