import React from 'react';
import { Row, Col } from 'react-bootstrap';
import Uploader from 'components/Uploader';
import { useUploadedDocuments } from 'hooks';
import { getDocumentUrl } from 'helpers/data';
import { useApplicationState } from '../Provider';

type Props = {
    disabled: boolean;
};

function Documents({ disabled }: Props) {
    const { applicationId: id } = useApplicationState();
    const passportUrl = getDocumentUrl('DOC_PASSPORT', id);
    const socCardUrl = getDocumentUrl('DOC_SOC_CARD', id);

    const { data: uploadedDocuments = [] } = useUploadedDocuments(id, {
        enabled: !!id
    });

    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">Փաստաթղթեր</h2>
                <Uploader
                    label="Անձը հաստատող փաստաթղթի պատճե"
                    url={passportUrl}
                    uploaded={uploadedDocuments.includes('DOC_PASSPORT')}
                    name="DOC_PASSPORT"
                    disabled={disabled}
                />
                <Uploader
                    label="Հանրային ծառայության համարանիշի (սոցիալական քարտի) պատճե"
                    url={socCardUrl}
                    uploaded={uploadedDocuments.includes('DOC_SOC_CARD')}
                    name="DOC_SOC_CARD"
                    disabled={disabled}
                />
            </Col>
        </Row>
    );
}

export default Documents;
