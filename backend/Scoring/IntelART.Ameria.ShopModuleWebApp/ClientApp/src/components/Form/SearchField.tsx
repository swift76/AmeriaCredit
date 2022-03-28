import React from 'react';
import { TextField } from 'components/Form';

const SearchField = ({ ...rest }) => {
    return (
        <div className="search-field">
            <TextField {...rest} />
        </div>
    );
};

export default SearchField;
