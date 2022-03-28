import { useEffect, useState } from 'react';
import { useFormContext } from 'react-hook-form';

export default function useReadonlyInputs(
    readOnly: boolean | undefined
): [string[], (name: string) => boolean] {
    const [inputs, setInputs] = useState<string[]>([]);
    const { getValues } = useFormContext();

    useEffect(() => {
        if (!readOnly) return;

        const values = getValues();

        // eslint-disable-next-line no-restricted-syntax
        for (const label in values) {
            if (values[label]) {
                setInputs(prev => [...prev, label]);
            }
        }
    }, [readOnly, getValues]);

    const isReadOnly = (name: string): boolean => inputs.includes(name);
    return [inputs, isReadOnly];
}
