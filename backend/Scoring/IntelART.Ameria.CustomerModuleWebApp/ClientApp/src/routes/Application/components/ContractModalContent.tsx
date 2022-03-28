import React from 'react';
import { Spinner } from 'react-bootstrap';

type Props = {
    statusId?: number;
    errorCode: string | null;
};

export default function ContractModalContent({ statusId, errorCode }: Props) {
    let content: string | React.ReactNode = ``;

    if (errorCode && errorCode === 'timeout') {
        content = `Առկա է տեխնիկական խնդիր, խնդրում ենք փորձել ավելի ուշ։ Ձեր հայտը պահպանվել է Հայտեր էջում։`;
    } else if (statusId === 21) {
        content = `Ձեր վարկը մուտքագրվել է Ձեր բանկային հաշվեհամարին։ Շնորհակալություն «Ամերիաբանկ» ՓԲԸ-ի ծառայություններից օգտվելու համար։ Ձեր վարկային փաթեթը հասանելի է հայտեր էջի «Գործողություն» հատվածում:`;
    } else {
        content = (
            <>
                <Spinner
                    animation="border"
                    variant="primary"
                    size="sm"
                    as="span"
                    className="float-left"
                />
                <p className="text-primary-darken pl-2 overflow-hidden mb-0">
                    Կատարվում են բանկային ձևակերպումները, խնդրում ենք սպասել, դա կարող է տևել մի
                    քանի րոպե:
                </p>
            </>
        );
    }

    return <div className="request-modal-content">{content}</div>;
}
