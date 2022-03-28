/* eslint-disable no-alert */
import React, { useState, useContext, useCallback } from 'react';
import { Button, Spinner } from 'react-bootstrap';
import { ActionTypes } from 'constants/application';
import clsx from 'clsx';
import { ApplicationListContext } from 'routes/AppliactoinList';
import { getOnBoardRedirectionLink, printApplication } from 'queryies';
import { openFile, getPrintApplicationUrl } from 'helpers/data';
import { toast } from 'react-toastify';
import { useRowActionType } from '../../hooks/use-row-action';

type Props = {
    id: string;
    status: string;
    onboardId: string | null;
};

const { APPROACHBANK, REMOVE, PRINT, CANCEL } = ActionTypes;

function TableActionCell({ id, status, onboardId }: Props) {
    const [loading, setLoading] = useState(false);
    const { setState } = useContext(ApplicationListContext);
    const { actionType } = useRowActionType(status);

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
            openFile(`assets/pdf/Terms and Condition_Consumer Finance.pdf`);
            setTimeout(() => {
                openFile(getPrintApplicationUrl(id));
            }, 400);
        } catch (error) {
            if (error && error.status === 500) {
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

    if (loading) {
        return <Spinner animation="border" size="sm" variant="primary" />;
    }

    if (!actionType) {
        return <>-</>;
    }

    if (actionType === APPROACHBANK && onboardId) {
        return (
            <Button
                variant="link"
                onClick={() => handleOnboardLoanClick(onboardId)}
                className={btnClassName}
            >
                Առանց Գրանցում
            </Button>
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
            {actionType === APPROACHBANK && `Մոտեցեք մասնաճյուղ`}
            {actionType === REMOVE && <i className="am-icon-delete" />}
            {actionType === CANCEL && <i className="am-icon-ban" />}
        </Button>
    );
}

export default TableActionCell;
