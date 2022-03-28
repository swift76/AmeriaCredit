import React from 'react';

type Props = {
    children: React.ReactNode;
};

function PageContainer({ children }: Props) {
    return <div className="tab-content">{children}</div>;
}

export default PageContainer;
