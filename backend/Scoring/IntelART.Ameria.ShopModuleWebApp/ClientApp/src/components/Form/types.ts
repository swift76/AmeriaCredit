import { FieldError, UseFormMethods } from 'react-hook-form';
import { FormControlProps } from 'react-bootstrap/FormControl';

export type FormInputBaseProps = React.DetailedHTMLProps<
    React.InputHTMLAttributes<HTMLInputElement>,
    HTMLInputElement
>;

export type FormControlElement = HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement;

export type InputOtherProps = {
    error?: FieldError;
    label?: React.ReactNode;
    regexp?: RegExp;
    withGroup?: boolean;
    setValue?: UseFormMethods['setValue'];
};

export type FormInputProps = FormInputBaseProps & FormControlProps & InputOtherProps;
