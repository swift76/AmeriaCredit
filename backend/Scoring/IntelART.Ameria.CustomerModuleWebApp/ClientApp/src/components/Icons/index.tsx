import React from 'react';
import { ReactComponent as Calendar } from 'assets/icons/calendar.svg';

type Props = {
    icon: string;
};

export default function Icon(props: Props) {
    const { icon, ...rest } = props;
    return <span className="svg-icon" />;
}
