import { IApplications } from 'types';

interface IConfig {
    id?: string;
    Header: string;
    accessor: keyof IApplications;
    title?: string;
}

export const columnsConfig: IConfig[] = [
    {
        id: 'name',
        Header: 'Վարկառու',
        accessor: 'NAME',
        title: 'Վարկառու'
    },
    {
        Header: 'Ստեղծված է',
        accessor: 'CREATION_DATE',
        title: 'Ստեղծված է'
    },
    {
        Header: 'Վարկի գումար',
        accessor: 'DISPLAY_AMOUNT',
        title: 'Վարկի գումար'
    },
    {
        Header: 'Կարգավիճակ',
        accessor: 'STATUS_AM',
        title: 'Կարգավիճակ'
    },
    {
        Header: 'Գործողություն',
        accessor: 'STATUS_STATE',
        title: 'Գործողություն'
    }
];
