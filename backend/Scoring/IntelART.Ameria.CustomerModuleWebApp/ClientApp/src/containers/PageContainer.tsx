import React from 'react';
import PageHeader from 'components/PageHeader';

type Props = {
    children: React.ReactNode;
    title?: string;
    action?: React.ReactNode;
};

function PageContainer({ children, title, action }: Props) {
    return (
        <div className="page-content">
            {title && <PageHeader title={title} rightContent={action} />}
            {children}
        </div>
    );
}

export default PageContainer;
