import React from 'react';
import { Spinner } from 'react-bootstrap';

type Props = {
    statusId?: number;
    errorCode: string | null;
};

export default function RequestModalContent({ statusId, errorCode }: Props) {
    let content: string | React.ReactNode = ``;

    if (errorCode) {
        switch (errorCode) {
            case 'ERR-0200':
                content = `Նշված համակարգով 8 օրվա ընթացքում հնարավոր է դիմել վարկի միայն 1 անգամ։ Խնդրում ենք վարկային հայտը ուղարկել 8 օր անց։ Շնորհակալություն «Ամերիաբանկ» ՓԲԸ-ի ծառայություններից օգտվելու համար։`;
                break;
            case 'ERR-0201':
                content = `Բանկի աշխատակիցներին արգելվում է օգտվել առցանց վարկավորման համակարգերից`;
                break;
            case 'timeout':
                content = `Առկա է տեխնիկական խնդիր, խնդրում ենք փորձել ավելի ուշ։ Ձեր հայտը պահպանվել է Հայտեր էջում։`;
                break;
            default:
                break;
        }
    } else {
        switch (statusId) {
            case undefined:
            case null:
            case 1:
            case 2:
            case 3:
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
                            Խնդրում ենք սպասել Ձեզ համար կատարվում է վարկի հարցում, որը կարող է տևել
                            մի քանի րոպե
                        </p>
                    </>
                );
                break;
            case 5:
                content = `Ձեզ համար հաստատվել է վարկ։ Վարկի պայմաններին ծանոթանալու համար խնդրում ենք սեղմել շարունակել կոճակը։ Ձեր հայտն ավտոմատ կչեղարկվի այն լրացնելուց հետո 7-րդ օրացուցային օրը, ուստի խնդրում ենք ավարտել վարկի ձևակերպման գործընթացը նշված ժամկետում։`;
                break;
            case 6:
                content = `Ձեր վարկը ավտոմատ սքորինգի համակարգով մերժվել է։`;
                break;
            case 7:
                content = (
                    <div>
                        Վարկի վերաբերյալ որոշում կայացնելու համար անհրաժեշտ են լրացուցիչ տվյալներ։
                        Խնդրում ենք անձը հաստատող փաստաթղթով, հանրային ծառայության համարանիշով և
                        աշխատավարձի տեղեկանքով մոտենալ «Ամերիաբանկ» ՓԲԸ-ի ցանկացած մասնաճյուղ։
                        «Ամերիաբանկ» ՓԲԸ-ի սպասարկման ցանցը և աշխատանքի ժամանակացույցը ներկայացված է{' '}
                        <a
                            href="http://ameriabank.am/Infrastructure.aspx?&lang=33"
                            target="_blank"
                            rel="noopener noreferrer"
                        >
                            հետևյալ հղումով:
                        </a>
                    </div>
                );
                break;

            default:
                break;
        }
    }

    return <div className="request-modal-content">{content}</div>;
}
