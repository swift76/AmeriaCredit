import React, { useState, useMemo, useEffect, useCallback } from 'react';
import { Modal, Button } from 'react-bootstrap';
import { CancelModalContent, ApproachBankModalContent } from 'components/Modals';
import LoadingButton from 'components/LoadingButton';
import { ActionTypes } from 'constants/application';
import { useQueryCache } from 'react-query';
import { deleteApplication, rejectApplication } from 'queryies';

type Props = {
    show: boolean;
    id: string;
    type: string;
    onClose: () => void;
};

const { Header, Title, Body, Footer } = Modal;

export default function ActionModal({ show, id, type, onClose }: Props) {
    const queryCach = useQueryCache();
    const [isSubmitting, setSubmitting] = useState(false);
    const [actionName, setActionName] = useState('');

    const handleCloseModal = () => {
        onClose();
        setSubmitting(false);
    };

    const handleActionClick = useCallback(async () => {
        setSubmitting(true);
        if (type === ActionTypes.REMOVE) {
            await deleteApplication(id);
        } else {
            await rejectApplication(id);
        }

        setSubmitting(false);
        queryCach.invalidateQueries('APPLICATIONS');
        handleCloseModal();
    }, [queryCach, handleCloseModal, type, id]);

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
        <Modal show={show} onHide={handleCloseModal}>
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
                <Button variant="secondary" onClick={handleCloseModal}>
                    Փակել
                </Button>
                {hasActionButton && (
                    <LoadingButton
                        variant="warning"
                        onClick={handleActionClick}
                        loading={isSubmitting}
                    >
                        {actionName}
                    </LoadingButton>
                )}
            </Footer>
        </Modal>
    );
}
