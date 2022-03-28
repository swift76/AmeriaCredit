import React, { useEffect, useState, useCallback } from 'react';
import { Col, Form, Row, Button, FormLabel } from 'react-bootstrap';
import { useForm } from 'react-hook-form';
import LoadingButton from 'components/LoadingButton';
import PageContainer from 'containers/PageContainer';
import { Link } from 'react-router-dom';
import { useUsername } from 'hooks';
import { TextField } from 'components/Form';
import { useAuthDispatch } from 'providers/AuthProvider';

type FormData = {
    Username: string;
    OldPassword: string;
    NewPassword: string;
    ConfirmNewPassword: string;
};

function ResetPassword() {
    const [loading, setLoading] = useState(false);
    const { handleSubmit, register, setValue } = useForm<FormData>();
    const { data } = useUsername();
    const { passwordReset } = useAuthDispatch();

    const username = data?.userName;
    const returnUrl = data?.returnUrl;

    const onSubmit = useCallback(
        async (formdata: FormData) => {
            try {
                setLoading(true);
                await passwordReset({ ...formdata, returnUrl });
            } finally {
                setLoading(false);
            }
        },
        [returnUrl, passwordReset]
    );

    useEffect(() => {
        if (username) {
            setValue('Username', username);
        }
    }, [username, setValue]);

    return (
        <PageContainer>
            <div className="reset-password">
                <Col sm={12}>
                    <h1 className="fieldset-title">Գաղտնաբառի փոփոխում</h1>
                </Col>
                <Form id="changePasswordForm" onSubmit={handleSubmit(onSubmit)}>
                    <Row>
                        <Col sm={3}>
                            <FormLabel>Մուտքանուն</FormLabel>
                        </Col>
                        <Col sm={4}>
                            <TextField ref={register} name="Username" readOnly />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={3}>
                            <FormLabel>Ընթացիկ գաղտնաբառ</FormLabel>
                        </Col>
                        <Col sm={4}>
                            <TextField ref={register} name="OldPassword" type="password" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={3}>
                            <FormLabel>Նոր գաղտնաբառ</FormLabel>
                        </Col>
                        <Col sm={4}>
                            <TextField ref={register} name="NewPassword" type="password" />
                        </Col>
                    </Row>
                    <Row>
                        <Col sm={3}>
                            <FormLabel>Կրկնել նոր գաղտնաբառը</FormLabel>
                        </Col>
                        <Col sm={4}>
                            <TextField ref={register} name="ConfirmNewPassword" type="password" />
                        </Col>
                    </Row>
                    <div className="form-actions text-right">
                        <Link to="\">
                            <Button variant="light">Չեղարկել</Button>
                        </Link>
                        <LoadingButton
                            type="submit"
                            variant="primary"
                            form="changePasswordForm"
                            loading={loading}
                        >
                            Հաստատել
                        </LoadingButton>
                    </div>
                </Form>
            </div>
        </PageContainer>
    );
}

export default ResetPassword;
