import React, { useEffect, useState } from 'react';
import TextField from '@mui/material/TextField';
import IconButton from '@mui/material/IconButton';
import SearchIcon from '@mui/icons-material/Search';
import { Box } from '@mui/material';

interface SearchProps {
    onSearch: (query: string) => void;
}

const Search: React.FC<SearchProps> = ({ onSearch }) => {
    const [searchQuery, setSearchQuery] = useState<string>('');

    const handleSearch = () => {
        onSearch(searchQuery);
    };

    const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            handleSearch();
        }
    };

    useEffect(() => {
        const timer = setTimeout(() => {
            onSearch(searchQuery);
        }, 500);

        return () => clearTimeout(timer);
    }, [searchQuery, onSearch]);

    return (
        <Box sx={{
            display: 'flex',
            gap: 1
        }}>
            <TextField
                id="input-with-sx"
                variant="standard"
                placeholder='Search...'
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onKeyPress={handleKeyPress}
            />
            <IconButton onClick={handleSearch} color='primary' aria-label="search">
                <SearchIcon sx={{ color: 'action.active', m: -0.5 }} />
            </IconButton>
        </Box>
    );
};

export default Search;
