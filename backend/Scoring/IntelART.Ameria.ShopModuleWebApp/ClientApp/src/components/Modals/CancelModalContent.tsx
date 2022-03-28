import React from 'react';

type Props = {
    actionName: string;
};

export default function LoadingModalContent({ actionName }: Props) {
    return (
        <p className="mb-0">
            {`Դուք պատրաստվում եք վերացնել հայտը։ Սեղմեք "${actionName}" կոճակը, եթե համաձայն եք,
            այլապես սեղմեք "Փակել" կոճակը։`}
        </p>
    );
}
