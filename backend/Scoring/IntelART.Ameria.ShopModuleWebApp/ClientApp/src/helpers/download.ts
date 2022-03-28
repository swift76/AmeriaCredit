/* eslint-disable func-names */
export const FileMimeTypeEnum = {
    EXCEL: 'application/vnd.ms-excel',
    PDF: 'application/pdf',
    HTML: 'text/html'
};

export const downloadFile = (
    blobPart: any,
    fileName: string,
    mimeType: keyof typeof FileMimeTypeEnum = 'PDF'
) => {
    const blob = new Blob([blobPart], { type: mimeType });

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob);
        return;
    }

    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', fileName);
    document.body.appendChild(link);
    link.click();

    setTimeout(function () {
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    }, 100);
};
