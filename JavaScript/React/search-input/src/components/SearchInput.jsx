import { useEffect, useRef, useState } from 'react';
import { SearchInputModes } from './SearchInputModes';

function SearchInput(props) {
    const {
        placeholder = 'Search',
        mode = SearchInputModes.IMMEDIATE,
        onSearch,
        delay = 500,
    } = props;

    const [value, setValue] = useState('');
    const timeoutRef = useRef(null);

    const handleChange = (event) => {
    const newValue = event.target.value;
    setValue(newValue);

    if (mode === SearchInputModes.IMMEDIATE) {
        onSearch(newValue);
    }
    };

    const handleKeyDown = (event) => {
    if (
        mode === SearchInputModes.ENTER &&
        event.key === 'Enter'
    ) {
        console.log('Enter was pressed');
        onSearch(value);
    }
    };

    useEffect(() => {
    if (mode !== SearchInputModes.DEBOUNCE) {
        return undefined;
    }

    clearTimeout(timeoutRef.current);

    timeoutRef.current = setTimeout(() => {
        onSearch(value);
    }, delay);
    
    return () => clearTimeout(timeoutRef.current);
    }, [value, mode, delay, onSearch]);

    return (
    <input
        type="text"
        value={value}
        placeholder={placeholder}
        onChange={handleChange}
        onKeyDown={handleKeyDown}
    />
    );

}

export default SearchInput;