import { QueryConfig, useQuery } from 'react-query';
import { getUploadedDocuments } from 'queryies';

export const queryKey = `UPLOADED_DOCUMENTS`;

export default function useUploadedDocuments(
    id?: string | null,
    config?: QueryConfig<Array<string>, any>
) {
    return useQuery<Array<string>, any>({
        queryKey: id && [queryKey, id],
        queryFn: getUploadedDocuments,
        config: {
            ...config
        }
    });
}
