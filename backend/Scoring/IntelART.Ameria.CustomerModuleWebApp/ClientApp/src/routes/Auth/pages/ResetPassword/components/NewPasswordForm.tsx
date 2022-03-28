import React, { useEffect, useState, useCallback, memo } from 'react';
import { Button, Form } from 'react-bootstrap';
import { useForm, Controller } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useAuthDispatch, useAuthState } from 'providers/AuthProvider';
import LoadingButton from 'components/LoadingButton';
import { TextField, RestrictedInput } from 'components/Form';

type FormData = {
    VerificationCode: string;
    Password: string;
    ConfirmPassword: string;
    RegistrationProcessId: string;
    Phone: string;
};

function NewPasswordForm() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const { passwordReset, resetAuthState } = useAuthDispatch();
    const { handleSubmit, errors, register, control, reset } = useForm<FormData>();
    const { processID, userPhone } = useAuthState();

    useEffect(() => {
        reset({
            RegistrationProcessId: processID,
            Phone: userPhone
        });
    }, [processID, userPhone]);

    const onSubmit = useCallback(
        async (formdata: FormData) => {
            try {
                setLoading(true);
                await passwordReset(formdata);
            } finally {
                setLoading(false);
            }
        },
        [passwordReset]
    );

    const goBack = () => {
        resetAuthState();
        navigate('/signin', { replace: true });
    };

    return (
        <Form className="auth-form" onSubmit={handleSubmit(onSubmit)}>
            <Controller
                name="VerificationCode"
                as={RestrictedInput}
                control={control}
                error={errors.VerificationCode}
                label="Խնդրում ենք մուտքագրել Ձեր բջջային հեռախոսին ուղարկված գաղտնաբառը։"
                type="text"
                maxLength={6}
            />

            <TextField
                name="Password"
                ref={register}
                type="password"
                error={errors.Password}
                label="Նոր գաղտնաբառ"
            />
            <TextField
                name="ConfirmPassword"
                type="password"
                ref={register}
                error={errors.ConfirmPassword}
                label="Կրկնել նոր գաղտնաբառը"
            />

            <TextField name="RegistrationProcessId" ref={register} type="hidden" />
            <TextField name="Phone" ref={register} type="hidden" />

            <hr />

            <Form.Group className="text-right mb-0">
                <Button variant="outline-primary" className="mr-1" onClick={goBack}>
                    Վերադառնալ
                </Button>
                <LoadingButton variant="primary" type="submit" loading={loading}>
                    Հաստատել
                </LoadingButton>
            </Form.Group>
        </Form>
    );
}

export default memo(NewPasswordForm);
