import React from 'react';
import { useForm, FormProvider } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import { IApplicationMain } from 'types';
import { yupResolver } from '@hookform/resolvers';
import LoadingButton from 'components/LoadingButton';

import { applicationMainScheme } from '../validations';
import ApplicationMainForm from '../components/ApplicationMainForm';

type Props = {
    disabled: boolean;
};

function ApplicationMain({ disabled }: Props) {
    const methods = useForm<IApplicationMain>({
        resolver: yupResolver(applicationMainScheme)
    });

    return (
        <FormProvider {...methods}>
            <ApplicationMainForm disabled={disabled} />
            <div className="form-actions text-right">
                <Link to="\">
                    <Button variant="light">Չեղարկել</Button>
                </Link>

                <LoadingButton
                    type="submit"
                    variant="primary"
                    form="ApplicationMainForm"
                    loading={false}
                    disabled={disabled}
                >
                    Հաստատել
                </LoadingButton>
            </div>
        </FormProvider>
    );
}

export default React.memo(ApplicationMain);
