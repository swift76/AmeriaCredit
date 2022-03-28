import React, { useState, useEffect } from 'react';
import { useForm, FormProvider } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import { IPreApplication } from 'types';
import { useApplication, useLoanLimits } from 'hooks';
import { yupResolver } from '@hookform/resolvers';
import LoadingButton from 'components/LoadingButton';
import PreApplicationForm from '../components/PreApplicationForm';
import { preApplicationScheme } from '../validations';
import { useApplicationState } from '../Provider';

type Props = {
    disabled: boolean;
};

function PreApplication({ disabled }: Props) {
    const loanTypeId = '00';
    const [validationContex, setValidationContext] = useState({
        loanLimits: null
    });
    const { applicationId } = useApplicationState();

    const methods = useForm<IPreApplication>({
        resolver: yupResolver(preApplicationScheme),
        context: validationContex
    });
    const { reset, watch, getValues } = methods;

    const currencyCode = watch(`CURRENCY_CODE`);

    const { data: applicationData, isLoading: isApplicationDataLoading } = useApplication(
        applicationId,
        {
            enabled: !!applicationId
        }
    );

    const { data: loanLimits } = useLoanLimits(loanTypeId, currencyCode);

    useEffect(() => {
        if (loanLimits) {
            setValidationContext((prev: any) => ({ ...prev, loanLimits }));
        }
    }, [loanLimits]);

    useEffect(() => {
        reset({
            ...applicationData,
            MOBILE_PHONE_AUTHORIZATION_CODE: getValues('MOBILE_PHONE_AUTHORIZATION_CODE')
        } as IPreApplication);
    }, [reset, applicationData]);

    return (
        <FormProvider {...methods}>
            <PreApplicationForm disabled={disabled} loading={isApplicationDataLoading} />
            <div className="form-actions text-right">
                <Link to="\">
                    <Button variant="light">Չեղարկել</Button>
                </Link>
                <LoadingButton
                    type="submit"
                    variant="primary"
                    form="PreApplicationForm"
                    loading={false}
                    disabled={disabled}
                >
                    Շարունակել
                </LoadingButton>
            </div>
        </FormProvider>
    );
}

export default React.memo(PreApplication);
