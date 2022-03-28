/* eslint-disable no-plusplus */
export const DATE_FORMAT_DEFAULT = 'dd/MM/yyyy';

export function getYearOptions() {
    const currentYear = new Date().getFullYear();
    const yearsOptions = [];
    for (let index = 0; index < 14; index++) {
        yearsOptions.push({ CODE: currentYear + index, NAME: currentYear + index });
    }
    return yearsOptions;
}
