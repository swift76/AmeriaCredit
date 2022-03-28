import React from 'react';
import { Row, Col } from 'react-bootstrap';
import Uploader from 'components/Uploader';
import { useUploadedDocuments } from 'hooks';
import { getDocumentUrl } from 'helpers/data';

type Props = {
    id: string;
};

function Documents({ id }: Props) {
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
                />
                <Uploader
                    label="Հանրային ծառայության համարանիշի (սոցիալական քարտի) պատճե"
                    url={socCardUrl}
                    uploaded={uploadedDocuments.includes('DOC_SOC_CARD')}
                    name="DOC_SOC_CARD"
                />
            </Col>
        </Row>
    );
}

export default Documents;
