import React, { useState, useMemo, useEffect, useCallback } from 'react';
import { Modal, Button } from 'react-bootstrap';
import { CancelModalContent, ApproachBankModalContent } from 'components/Modals';
import LoadingButton from 'components/LoadingButton';
import { ActionTypes } from 'constants/application';
import { useQueryCache } from 'react-query';
import { deleteApplication, cancelApplication } from 'queryies';

type Props = {
    show: boolean;
    id: string;
    type: string;
    onClose: () => void;
};

const { Header, Title, Body, Footer } = Modal;

export default function ActionModal({ show, id, type, onClose }: Props) {
    const queryCach = useQueryCache();
    const [isSubmitting, setSubmittin] = useState(false);
    const [actionName, setActionName] = useState('');

    const handleActionClick = useCallback(async () => {
        setSubmittin(true);
        await (type === ActionTypes.REMOVE ? deleteApplication(id) : cancelApplication(id));
        setSubmittin(false);
        queryCach.invalidateQueries('APPLICATIONS');
        onClose();
    }, [queryCach, onClose, type, id]);

    useEffect(() => {
        switch (type) {
            case ActionTypes.REMOVE:
                setActionName('Ջնջել');
                break;
            case ActionTypes.CANCEL:
                setActionName('Չեղարկել');
                break;
            case ActionTypes.APPROACHBANK:
                setActionName('Մոտեցեք մասնաճյուղ');
                break;

            default:
                break;
        }
    }, [type]);

    const hasActionButton = useMemo(() => {
        return type === ActionTypes.REMOVE || type === ActionTypes.CANCEL;
    }, [type]);

    return (
        <Modal show={show} onHide={onClose}>
            <Header closeButton>
                <Title>{actionName}</Title>
            </Header>
            <Body>
                {hasActionButton ? (
                    <CancelModalContent actionName={actionName} />
                ) : (
                    <ApproachBankModalContent />
                )}
            </Body>
            <Footer>
                {hasActionButton && (
                    <LoadingButton
                        variant="primary"
                        onClick={handleActionClick}
                        loading={isSubmitting}
                    >
                        {actionName}
                    </LoadingButton>
                )}
                <Button variant="secondary" onClick={onClose}>
                    Փակել
                </Button>
            </Footer>
        </Modal>
    );
}
