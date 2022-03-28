import * as yup from 'yup';
import qs from 'qs';
import React, { useEffect, useState, useCallback, memo } from 'react';
import { Col, Form } from 'react-bootstrap';
import { yupResolver } from '@hookform/resolvers';
import { useForm, Controller } from 'react-hook-form';
import { getOnBoardingDataFrom } from 'queryies';
import { onlyEng } from 'validators/regexp';
import { termsLink } from 'constants/links';
import { RestrictedInput, TextField, PhoneNumberInput } from 'components/Form';
import { FieldSet } from 'components/fieldset';
import { useLocation } from 'react-router-dom';
import { useAuthDispatch } from 'providers/AuthProvider';
import LoadingButton from 'components/LoadingButton';

type FormData = {
    Name: string;
    Lastname: string;
    SSN: string;
    Phone: string;
    Email: string;
    Password: string;
    ConfirmPassword: string;
    AcceptedTermsAndConditions: boolean;
    ONBOARDING_ID?: string;
};

const schema = yup.object().shape({
    Name: yup.string().required(),
    Lastname: yup.string().required(),
    SSN: yup
        .string()
        .test(
            'len',
            'Նման ՀԾՀՀ / սոցիալական քարտի համար գոյություն չունի',
            val => val?.length === 10
        ),
    Phone: yup.string().required(),
    Email: yup.string().email().required(),
    Password: yup
        .string()
        .required()
        .matches(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/,
            'Գաղտնաբառը պետք է պարունակի ամենաքիչը 8 նշան, մեկ մեծատառ տառ, մեկ փոքրատառ տառ, մեկ թվանշան և մեկ նշան'
        ),
    ConfirmPassword: yup
        .string()
        .oneOf([yup.ref('Password'), ' '], 'PASSWORDS_MATCH')
        .required()
});

function RegisterForm() {
    const { search } = useLocation();
    const [onBoardingLoading, setOnBoardingLoading] = useState(false);
    const { onBoardingId } = qs.parse(search, { ignoreQueryPrefix: true });
    const [loading, setLoading] = useState(false);
    const [hasOnboardData, setHasOnBoardData] = useState(false);
    const { register: registerUser } = useAuthDispatch();
    const { register, handleSubmit, errors, watch, control, reset } = useForm<FormData>({
        resolver: yupResolver(schema)
    });
    const [isStudent, setIsStudent] = useState(false);
    const acceptedTerms = watch('AcceptedTermsAndConditions', false);

    const onSubmit = useCallback(
        async (formdata: FormData) => {
            try {
                setLoading(true);
                await registerUser({
                    ...formdata,
                    IS_STUDENT: isStudent
                });
            } catch (error) {
                setLoading(false);
            }
        },
        [registerUser, isStudent]
    );
    useEffect(() => {
        async function getOnBoardData(id: string) {
            setOnBoardingLoading(true);
            try {
                const onBoardData = await getOnBoardingDataFrom(null, id);

                if (onBoardData) {
                    reset({
                        Name: onBoardData.first_name_eng,
                        Lastname: onBoardData.last_name_eng,
                        SSN: onBoardData.soccard_number,
                        Phone: onBoardData.mobile_number,
                        Email: onBoardData.email,
                        ONBOARDING_ID: onBoardData.id
                    });
                    setIsStudent(onBoardData.is_student);
                }
            } finally {
                setOnBoardingLoading(false);
                setHasOnBoardData(true);
            }
        }

        if (onBoardingId) {
            getOnBoardData(onBoardingId as string);
        }
    }, [reset, onBoardingId]);

    return (
        <Form className="auth-form" onSubmit={handleSubmit(onSubmit)}>
            <FieldSet disabled={hasOnboardData} loading={onBoardingLoading}>
                <Form.Row className="mb-3">
                    <Col xs={12} sm={6}>
                        <Controller
                            name="Name"
                            as={RestrictedInput}
                            control={control}
                            error={errors.Name}
                            label="Անուն (անգլերեն)"
                            regexp={onlyEng}
                        />
                    </Col>

                    <Col xs={12} sm={6}>
                        <Controller
                            name="Lastname"
                            as={RestrictedInput}
                            control={control}
                            error={errors.Lastname}
                            label="Ազգանուն (անգլերեն)"
                            regexp={onlyEng}
                        />
                    </Col>
                </Form.Row>

                <Controller
                    name="SSN"
                    as={RestrictedInput}
                    control={control}
                    error={errors.SSN}
                    label="ՀԾՀՀ / սոցիալական քարտի համար"
                    type="number"
                    maxLength={10}
                />

                <Form.Row>
                    <Col xs={12} sm={6}>
                        <PhoneNumberInput
                            name="Phone"
                            error={errors.Phone}
                            control={control}
                            autoComplete="username"
                            rules={{ required: true }}
                        />
                    </Col>
                    <Col xs={12} sm={6}>
                        <TextField
                            name="Email"
                            ref={register}
                            error={errors.Email}
                            label="Էլ․ հասցե"
                            type="text"
                        />
                    </Col>
                </Form.Row>
            </FieldSet>

            <TextField
                type="password"
                ref={register}
                name="Password"
                error={errors.Password}
                label="Գաղտնաբառ"
            />

            <TextField
                type="password"
                ref={register}
                name="ConfirmPassword"
                error={errors.ConfirmPassword}
                label="Կրկնել գաղտնաբառ"
            />

            <TextField name="ONBOARDING_ID" ref={register} type="hidden" />

            <Form.Group controlId="AcceptedTermsAndConditionsControl">
                <Form.Check
                    type="checkbox"
                    ref={register}
                    label={
                        <>
                            Համաձայն եմ
                            <a
                                href={termsLink}
                                target="_blank"
                                rel="noopener noreferrer"
                                className="ml-1"
                            >
                                պայմաններին և կանոններին
                            </a>
                        </>
                    }
                    name="AcceptedTermsAndConditions"
                />
            </Form.Group>

            <hr />

            <Form.Group className="text-right mb-0">
                <LoadingButton
                    variant="primary"
                    type="submit"
                    disabled={!acceptedTerms || loading}
                    loading={loading}
                >
                    Գրանցվել
                </LoadingButton>
            </Form.Group>
        </Form>
    );
}

export default memo(RegisterForm);
