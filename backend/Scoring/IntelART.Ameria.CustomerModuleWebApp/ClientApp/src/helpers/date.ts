import { ISelectOption } from 'types';

export function toUtcMidnight(date: Date | undefined = undefined): string | undefined {
    if (date) {
        date.setHours(date.getHours() - date.getTimezoneOffset() / 60);
    }
    return date ? date.toISOString() : undefined;
}

export function getMonthOptions(): ISelectOption[] {
    const expiryMonthOptions: ISelectOption[] = [];

    // eslint-disable-next-line no-plusplus
    for (let i = 1; i < 13; i++) {
        expiryMonthOptions.push({ value: `${i}`, name: `${i}` });
    }

    return expiryMonthOptions;
}

export function getExpiryYearOptions(): ISelectOption[] {
    const expiryYearOptions: ISelectOption[] = [];
    const currentYear = new Date().getFullYear();

    // eslint-disable-next-line no-plusplus
    for (let i = 0; i < 14; i++) {
        expiryYearOptions.push({ value: `${currentYear + i}`, name: `${currentYear + i}` });
    }

    return expiryYearOptions;
}
