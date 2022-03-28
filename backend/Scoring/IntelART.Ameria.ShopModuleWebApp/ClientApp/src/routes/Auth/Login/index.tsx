import React, { memo, useState, useCallback } from 'react';
import { Form } from 'react-bootstrap';
import { useForm } from 'react-hook-form';
import { useAuthDispatch } from 'providers/AuthProvider';
import { TextField } from 'components/Form';
import LoadingButton from 'components/LoadingButton';
import Logo from 'components/Logo';

type FormData = {
    Username: string;
    Password: string;
};

function Login() {
    const [loginLoading, setLoginLoading] = useState(false);
    const { login } = useAuthDispatch();
    const { handleSubmit, errors, register } = useForm<FormData>();

    const onSubmit = useCallback(
        async ({ Username, Password }: FormData) => {
            try {
                setLoginLoading(true);
                await login({ Username, Password });
            } catch (error) {
                setLoginLoading(false);
            }
        },
        [login]
    );

    return (
        <div className="signin">
            <Form className="auth-form" onSubmit={handleSubmit(onSubmit)}>
                <Logo />
                <h4>Ապառիկ հայտերի առցանց համակարգ</h4>
                <TextField
                    name="Username"
                    ref={register({ required: true })}
                    label="Օգտատեր"
                    error={errors.Username}
                />

                <TextField
                    name="Password"
                    type="password"
                    ref={register({ required: true })}
                    label="Գաղտնաբառ"
                    error={errors.Password}
                    autoComplete="current-password"
                />

                <Form.Group>
                    <LoadingButton
                        className="auth-btn"
                        variant="primary"
                        type="submit"
                        loading={loginLoading}
                    >
                        Մուտք
                    </LoadingButton>
                </Form.Group>
            </Form>
        </div>
    );
}

export default memo(Login);
