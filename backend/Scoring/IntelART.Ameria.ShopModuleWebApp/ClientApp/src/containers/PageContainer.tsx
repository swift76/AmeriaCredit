import React from 'react';

type Props = {
    children: React.ReactNode;
};

function PageContainer({ children }: Props) {
    return <div className="page-content">{children}</div>;
}

export default PageContainer;
