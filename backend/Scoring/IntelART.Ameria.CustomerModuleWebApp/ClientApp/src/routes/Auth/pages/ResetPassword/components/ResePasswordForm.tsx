import React, { useState, useCallback, memo } from 'react';
import { Button, Form, InputGroup } from 'react-bootstrap';
import { useForm, Controller } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useAuthDispatch } from 'providers/AuthProvider';
import LoadingButton from 'components/LoadingButton';
import { RestrictedInput } from 'components/Form';

type FormData = {
    Phone: string;
};

function ResePasswordForm() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const { requestPasswordReset, resetAuthState } = useAuthDispatch();
    const { control, handleSubmit, errors } = useForm<FormData>();

    const onSubmit = useCallback(
        async (formdata: FormData) => {
            setLoading(true);
            try {
                await requestPasswordReset(formdata);
            } catch {
                setLoading(false);
            }
        },
        [requestPasswordReset]
    );

    const goBack = () => {
        resetAuthState();
        navigate('/signin', { replace: true });
    };

    return (
        <Form className="auth-form" onSubmit={handleSubmit(onSubmit)}>
            <Form.Group>
                <Form.Label>
                    Եթե չեք հիշում Ձեր գաղտնաբառը, մուտքագրեք այն հեռախոսահամարը, որն օգտագործել եք
                    գրանցման ժամանակ, և այդ համարին կուղարկվի քառանիշ կոդ՝ գաղտնաբառը վերականգնելու
                    համար
                </Form.Label>
                <InputGroup>
                    <InputGroup.Prepend>
                        <InputGroup.Text>374</InputGroup.Text>
                    </InputGroup.Prepend>
                    <Controller
                        name="Phone"
                        as={RestrictedInput}
                        control={control}
                        error={errors.Phone}
                        type="number"
                        withGroup={false}
                        placeholder="XX XXX XXX"
                        maxLength={8}
                        rules={{ required: true }}
                    />
                </InputGroup>
            </Form.Group>

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

export default memo(ResePasswordForm);
