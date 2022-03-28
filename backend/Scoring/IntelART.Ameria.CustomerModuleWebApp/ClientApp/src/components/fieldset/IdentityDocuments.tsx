import React, { useMemo } from 'react';
import { Row, Col } from 'react-bootstrap';
import { RestrictedInput, TextField, SelectField } from 'components/Form';
import { useFormContext, Controller } from 'react-hook-form';
import { useDocumentTypes } from 'hooks';
import { buildSelectOption } from 'helpers/data';

type Props = {
    name?: string;
    isDataOnBoard?: boolean;
};

function IdentityDocuments({ name, isDataOnBoard }: Props) {
    const { register, errors, control, watch } = useFormContext();
    const { isLoading: isDocumentTypesLoading, data: documentTypes } = useDocumentTypes();
    const documentTypeCode = watch('DOCUMENT_TYPE_CODE');
    const isIdCardSelected = documentTypeCode === '2';

    const documentTyesOptions = useMemo(() => {
        return buildSelectOption(documentTypes || []);
    }, [documentTypes]);

    return (
        <Row className="mb-3">
            <Col sm={12}>
                <h2 className="fieldset-title">Անձը հաստատող փաստաթղթեր</h2>
            </Col>
            <Col sm={6}>
                <TextField
                    ref={register}
                    name="SOCIAL_CARD_NUMBER"
                    label="Հանրային ծառայությունների համարանիշ / սոցիալական քարտ"
                    error={errors?.SOCIAL_CARD_NUMBER}
                    readOnly
                />
            </Col>
            <Col sm={6} />
            <Col sm={6}>
                <Controller
                    as={SelectField}
                    control={control}
                    name="DOCUMENT_TYPE_CODE"
                    label="Փաստաթղթի տեսակ"
                    options={documentTyesOptions}
                    error={errors?.DOCUMENT_TYPE_CODE}
                    loading={isDocumentTypesLoading}
                    disabled={isDataOnBoard}
                />
            </Col>
            <Col sm={6}>
                <Controller
                    as={RestrictedInput}
                    control={control}
                    name="DOCUMENT_NUMBER"
                    label="Փաստաթղթի համար"
                    error={errors?.DOCUMENT_NUMBER}
                    maxLength={9}
                    type={isIdCardSelected ? 'number' : 'text'}
                    readOnly={isDataOnBoard}
                />
            </Col>
        </Row>
    );
}

export default IdentityDocuments;
