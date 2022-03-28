import React, { useState, useContext, useCallback } from 'react';
import { Button, Spinner } from 'react-bootstrap';
import { ActionTypes } from 'constants/application';
import clsx from 'clsx';
import { ApplicationListContext } from 'routes/AppliactoinList';
import { getOnBoardRedirectionLink, printApplication } from 'queryies';
import { useRowActionType } from '../../hooks/use-row-action';
import { useOnBoardingSettings } from 'hooks';
import { toast } from 'react-toastify';

type Props = {
    id: string;
    status: string;
};

const { APPROACHBANK, REMOVE, PRINT, CANCEL } = ActionTypes;

function TableActionCell({ id, status }: Props) {
    const [loading, setLoading] = useState(false);
    const { setState } = useContext(ApplicationListContext);
    const { actionType } = useRowActionType(status);
    const { data: isRedirectAllowed } = useOnBoardingSettings();

    const handleOnboardLoanClick = async (applicationId: string) => {
        setLoading(true);
        try {
            const redirectionLink = await getOnBoardRedirectionLink(applicationId);
            if (redirectionLink) {
                window.open(redirectionLink, '_blank');
            }
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    const printApplicationDocuments = async (event: any) => {
        event.stopPropagation();
        try {
            await printApplication(id);
            const win = window.open(
                `/api/loan/Applications/${id}/Documents/DOC_LOAN_CONTRACT_FINAL`
            );
            (win as any).print();
        } catch (error: any) {
            if (error.status === 500) {
                toast.error('Պայմանագրի տպելու ձևը դեռ պատրաստ չէ');
            }
        }
    };

    const handleClick = useCallback(
        async (event: any) => {
            event.stopPropagation();
            setState({ id, type: actionType });
        },
        [setState, actionType, id]
    );

    const btnClassName = clsx('btn-sm', actionType && `btn-${actionType.toLowerCase()}`);

    if (!actionType) {
        return <>-</>;
    }

    if (actionType === APPROACHBANK) {
        return (
            <>
                {isRedirectAllowed && (
                    <Button
                        variant="primary"
                        disabled={loading}
                        onClick={e => {
                            e.stopPropagation();
                            handleOnboardLoanClick(id);
                        }}
                        className={btnClassName}
                        style={{ marginRight: 10, marginBottom: 10 }}
                    >
                        {loading && <Spinner animation="border" size="sm" variant="primary" />}{' '}
                        Առանց Գրանցում
                    </Button>
                )}
                <Button variant="primary" onClick={handleClick} className={btnClassName}>
                    Մոտեցեք մասնաճյուղ
                </Button>
            </>
        );
    }

    if (actionType === PRINT) {
        return (
            <Button variant="link" onClick={printApplicationDocuments} className={btnClassName}>
                <i className="am-icon-print" />
            </Button>
        );
    }

    return (
        <Button variant="link" onClick={handleClick} className={btnClassName}>
            {actionType === REMOVE && <i className="am-icon-delete" />}
            {actionType === CANCEL && <i className="am-icon-ban" />}
        </Button>
    );
}

export default TableActionCell;
