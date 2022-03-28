import React, { useState, useCallback, memo } from 'react';
import { Form } from 'react-bootstrap';
import { useForm, Controller } from 'react-hook-form';
import { useAuthDispatch, useAuthState } from 'providers/AuthProvider';
import LoadingButton from 'components/LoadingButton';
import { RestrictedInput } from 'components/Form';

type FormData = {
    VerificationCode: string;
};
type Props = {};

const VerifyRegisterForm: React.FC<Props> = () => {
    const { processID } = useAuthState();
    const { verifyPhone, resendCode } = useAuthDispatch();
    const [verifyLoading, setVerifyLoading] = useState(false);
    const [resendLoading, setResendLoading] = useState(false);
    const { control, handleSubmit, errors, reset: resetForm } = useForm<FormData>();

    const onSubmit = useCallback(
        async (formdata: FormData) => {
            try {
                setVerifyLoading(true);
                await verifyPhone({ ...formdata, registrationProcessID: processID });
            } finally {
                setVerifyLoading(false);
            }
        },
        [verifyPhone, processID]
    );

    const handleSendCodeAgain = useCallback(async () => {
        resetForm();
        setResendLoading(true);
        await resendCode({ processID });
        setResendLoading(false);
    }, [resendCode, processID, resetForm]);

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
                rules={{ required: true }}
            />

            <hr />

            <Form.Group className="text-right mb-0">
                <LoadingButton
                    variant="outline-primary"
                    loading={resendLoading}
                    className="mr-1"
                    onClick={handleSendCodeAgain}
                >
                    Կրկին ուղարկել
                </LoadingButton>

                <LoadingButton variant="primary" type="submit" loading={verifyLoading}>
                    Հաստատել
                </LoadingButton>
            </Form.Group>
        </Form>
    );
};

export default memo(VerifyRegisterForm);
