export const decodeToken = (token: string | null) => {
    if (!token) return '';
    const [, base64Url] = token.split('.');
    const base64 = base64Url.replace('-', '+').replace('_', '/');
    const tokenData = JSON.parse(window.atob(base64));
    return tokenData;
};
