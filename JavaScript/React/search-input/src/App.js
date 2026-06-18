import './App.css';
import SearchInput from './components/SearchInput';
import {SearchInputModes} from './components/SearchInputModes'

function App() {
  const handleSearch = (value) => {
    console.log('Searching:', value);
  };
  
  return (
    <div>
      <h2>Immediate</h2>
      <SearchInput
        placeholder="Search..."
        mode={SearchInputModes.IMMEDIATE}
        onSearch={handleSearch}
      />

      <h2>Enter</h2>
      <SearchInput
        placeholder="Search..."
        mode={SearchInputModes.ENTER}
        onSearch={handleSearch}
      />

      <h2>Debounce</h2>
      <SearchInput
        placeholder="Search..."
        mode={SearchInputModes.DEBOUNCE}
        delay={500}
        onSearch={handleSearch}
      />
    </div>
  );
  }

export default App;
