export function isObject(a: Object | null) {
    return a && typeof a === 'object' && !Array.isArray(a);
}
