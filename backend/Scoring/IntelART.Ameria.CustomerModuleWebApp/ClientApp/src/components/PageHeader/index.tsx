import React from 'react';

type Props = {
    title: React.ReactNode;
    rightContent?: React.ReactNode;
};

export default function PageHeader({ title, rightContent }: Props) {
    return (
        <div className="page-headings">
            <h4 className="mb-0">{title}</h4>
            {rightContent}
        </div>
    );
}
