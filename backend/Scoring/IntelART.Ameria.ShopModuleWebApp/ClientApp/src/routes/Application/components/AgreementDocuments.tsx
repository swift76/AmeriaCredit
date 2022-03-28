import React from 'react';
import { Row, Col, Button } from 'react-bootstrap';
import { useFormContext } from 'react-hook-form';
import { IPreApplication } from 'types/dataTypes/application';
import Uploader from 'components/Uploader';
import { FieldSet } from 'components/fieldset';
import { useUploadedDocuments } from 'hooks';
import { getDocumentUrl, openFile } from 'helpers/data';
import { downloadFile } from 'helpers/download';
import { saveApplication } from 'queryies';
import API from 'services/api';
import { useApplicationState, useApplicationDispatch } from '../Provider';

type Props = {
    disabled: boolean;
};

function AgreementDocuments({ disabled }: Props) {
    const { applicationId } = useApplicationState();
    const { setApplicationId } = useApplicationDispatch();
    const getRequestAgreement = (id: string) => getDocumentUrl('DOC_SCORING_REQUEST_AGREEMENT', id);
    const getRequestAgreementSigned = (id: string) =>
        getDocumentUrl('DOC_SCORING_REQUEST_AGREEMENT_SIGNED', id);
    const { getValues, trigger } = useFormContext();

    const { data: uploadedDocuments = [] } = useUploadedDocuments(applicationId, {
        enabled: !!applicationId
    });

    const handleSave = async () => {
        const formData = getValues();

        const { data } = await saveApplication({
            ...formData,
            LOAN_TYPE_ID: '00',
            SUBMIT: false,
            ORGANIZATION_ACTIVITY_CODE: ''
        } as IPreApplication);

        setApplicationId(data);
        openFile(getRequestAgreement(data));
    };

    const handleRequestAgreementDownload = async () => {
        const result = await trigger(
            Object.keys(getValues()).filter(
                item =>
                    item !== 'MOBILE_PHONE_AUTHORIZATION_CODE' &&
                    item !== 'DOC_SCORING_REQUEST_AGREEMENT_SIGNED'
                // item !== 'PRODUCT_CATEGORY_CODE' &&
                // item !== 'LOAN_TEMPLATE_CODE'
            )
        );

        if (result) {
            if (applicationId) {
                openFile(getRequestAgreement(applicationId));
            } else {
                handleSave();
            }
        }
    };

    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">Փաստաթղթեր</h2>
            </Col>
            <Col sm={6} className="mb-3">
                <Button variant="info" onClick={handleRequestAgreementDownload} disabled={disabled}>
                    Տպել հարցումների համաձայնությունը
                </Button>
            </Col>
            <Col sm={12}>
                <FieldSet disabled={!applicationId}>
                    <Uploader
                        label="Կցել հարցումների համաձայնությունը"
                        url={getRequestAgreementSigned(applicationId)}
                        uploaded={uploadedDocuments.includes(
                            'DOC_SCORING_REQUEST_AGREEMENT_SIGNED'
                        )}
                        name="DOC_SCORING_REQUEST_AGREEMENT_SIGNED"
                        disabled={disabled}
                    />
                </FieldSet>
            </Col>
        </Row>
    );
}

export default AgreementDocuments;
