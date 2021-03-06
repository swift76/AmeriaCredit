import React, { memo, useState, useCallback } from 'react';
import { Form, Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { useAuthDispatch } from 'providers/AuthProvider';
import { TextField, PhoneNumberInput } from 'components/Form';
import LoadingButton from 'components/LoadingButton';
import { getProfile } from 'queryies';
import { setSession } from 'services/api';

type FormData = {
    Username: string;
    Password: string;
};

function Login() {
    const [shouldVerifyEmail, setShoultVerifyEmail] = useState(false);
    const [loginLoading, setLoginLoading] = useState(false);
    const { login, setIsAutentificated } = useAuthDispatch();
    const { handleSubmit, errors, register, control } = useForm<FormData>();

    const onSubmit = useCallback(
        async ({ Username, Password }: FormData) => {
            setShoultVerifyEmail(false);
            try {
                setLoginLoading(true);
                await login({ Username, Password });
                const profileData = await getProfile();

                // login user only if IS_EMAIL_VERIFIED true or null (for new user IS_EMAIL_VERIFIED is false for old users null)

                if (
                    profileData?.IS_EMAIL_VERIFIED === null ||
                    profileData?.IS_EMAIL_VERIFIED === true
                ) {
                    setIsAutentificated();
                } else {
                    setSession(null);
                    setLoginLoading(false);
                    setShoultVerifyEmail(true);
                }
            } catch (error) {
                setLoginLoading(false);
            }
        },
        [login, setIsAutentificated]
    );

    return (
        <div className="signin">
            <Form className="auth-form" onSubmit={handleSubmit(onSubmit)}>
                {shouldVerifyEmail && (
                    <Form.Group>
                        <Alert
                            variant="danger"
                            onClose={() => setShoultVerifyEmail(false)}
                            dismissible
                            className="text-center"
                        >
                            ?????????????? ?????? ???????????????? ?????? ??????? ????????????, ???????????????? ?????? ?????????????????? ???????????? ??????
                            ???????????? ????????????
                        </Alert>
                    </Form.Group>
                )}
                <PhoneNumberInput
                    name="Username"
                    error={errors.Username}
                    control={control}
                    autoComplete="username"
                    rules={{ required: true }}
                />

                <TextField
                    name="Password"
                    type="password"
                    ref={register({ required: true })}
                    label="??????????????????"
                    error={errors.Password}
                    autoComplete="current-password"
                />

                <Form.Group>
                    <Link to="/reset-password">?????????????????????? / ?????????? ????????????????????</Link>
                </Form.Group>

                <hr />

                <Form.Group className="text-right mb-0">
                    <LoadingButton variant="primary" type="submit" loading={loginLoading}>
                        ??????????
                    </LoadingButton>
                </Form.Group>
            </Form>
        </div>
    );
}

export default memo(Login);
