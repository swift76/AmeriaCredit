import React, { useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import { useForm, FormProvider } from 'react-hook-form';
import { useMutation, useQueryCache } from 'react-query';
import Button from 'react-bootstrap/Button';
import { Link } from 'react-router-dom';
import { IProfileData } from 'types';
import { termsLink } from 'constants/links';
import { saveProfile } from 'queryies';
import { AddressDetails, IdentityDocuments, PersonalDetails } from 'components/fieldset';
import LoadingButton from 'components/LoadingButton';
import { useUserState } from 'providers/UserProvider';

function ProfileForm() {
    const profileData = useUserState();
    const queryCache = useQueryCache();
    const methods = useForm<IProfileData>();
    const { handleSubmit, reset } = methods;
    const onBoardingId = profileData?.ONBOARDING_ID;

    const [mutateProfile, { isLoading: isProfileSubmitting }] = useMutation(saveProfile, {
        onSuccess: () => {
            queryCache.invalidateQueries('PROFILE');
        }
    });

    const onSubmit = (values: IProfileData) => {
        mutateProfile(values);
    };

    useEffect(() => {
        if (profileData) {
            reset({ ...profileData });
        }
    }, [reset, profileData]);

    return (
        <>
            <FormProvider {...methods}>
                <Form onSubmit={handleSubmit(onSubmit)} id="ProfileForm">
                    <PersonalDetails
                        nameAM
                        nameEN
                        patronicName
                        birthDate
                        citizenShip
                        mobilePhone
                        email
                        isDataOnBoard={!!onBoardingId}
                    />
                    <IdentityDocuments isDataOnBoard={!!onBoardingId} />
                    <AddressDetails namePrefix="REGISTRATION" title="Գրանցման հասցե" />
                    <AddressDetails namePrefix="CURRENT" title="Բնակության հասցե" />
                </Form>
            </FormProvider>
            <div className="form-actions">
                <Button variant="outline-primary" as={Link} to="/">
                    Չեղարկել
                </Button>
                <LoadingButton
                    type="submit"
                    variant="primary"
                    form="ProfileForm"
                    loading={isProfileSubmitting}
                >
                    Հիշել
                </LoadingButton>
                <a href={termsLink} className="d-block mt-4 mb-4">
                    Անձնական էջի բացման և սպասարկման Պայմաններ և Կանոններ
                </a>
            </div>
        </>
    );
}

export default ProfileForm;
