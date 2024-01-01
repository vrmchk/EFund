import React, { useEffect } from 'react';
import Autocomplete from '@mui/material/Autocomplete';
import TextField from '@mui/material/TextField';
import Chip from '@mui/material/Chip';

const MultiSelectWithChip = (props: { names: string[], onChange: (selected: string[]) => void }) => {
    const [selectedOptions, setSelectedOptions] = React.useState<string[]>([]);

    useEffect(() => {
        props.onChange(selectedOptions);
    }, [props, selectedOptions]);

    const handleChange = (event: React.SyntheticEvent, value: string[]) => {
        setSelectedOptions(value);
    };

    return (
        <div
            style={{
                width: '250px',
            }}
        >
            <Autocomplete
                multiple
                limitTags={2}
                options={props.names}
                value={selectedOptions}
                onChange={handleChange}
                renderInput={(params) => (
                    <TextField
                        {...params}
                        label=""
                        variant="standard"
                    />
                )}
                renderTags={(value, getTagProps) =>
                    value.map((option, index) => (
                        <Chip
                            size='small'
                            color='primary'
                            label={option}
                            {...getTagProps({ index })}
                        />
                    ))
                }
            />
        </div>
    );
};

export default MultiSelectWithChip;