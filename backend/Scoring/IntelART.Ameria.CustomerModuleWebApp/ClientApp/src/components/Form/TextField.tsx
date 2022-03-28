import React, { Fragment } from 'react';
import FormGroup from 'react-bootstrap/FormGroup';
import FormLabel from 'react-bootstrap/FormLabel';
import FormControl from 'react-bootstrap/FormControl';
import { FormInputProps } from './types';

const TextField = React.forwardRef<HTMLInputElement, FormInputProps>(
    ({ error, label, withGroup = true, ...rest }, ref) => {
        const Group = withGroup ? FormGroup : Fragment;
        if (Group === FormGroup) {
            (FormGroup as FormGroup).defaultProps = {
                controlId: `${rest.name}_Control`
            };
        }
        return (
            <Group>
                {label && <FormLabel>{label}</FormLabel>}
                <FormControl ref={ref} isInvalid={!!error} {...rest} />
                <FormControl.Feedback type="invalid">{error?.message}</FormControl.Feedback>
            </Group>
        );
    }
);

export default TextField;
