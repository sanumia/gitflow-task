import './App.css';
import SearchInput from './components/SearchInput';
import {SearchInputModes} from './components/SearchInputModes'

const searchConfigs = [
  { mode: SearchInputModes.IMMEDIATE, label: 'Immediate' },
  { mode: SearchInputModes.ENTER, label: 'Enter' },
  { mode: SearchInputModes.DEBOUNCE, label: 'Debounce', delay: 500 },
];

function App() {
  const handleSearch = (value) => {
    console.log('Searching:', value);
  };

  return (
    <div>
      {searchConfigs.map(({ mode, label, delay }) => (
        <div key={mode}>
          <h2>{label}</h2>
          <SearchInput
            placeholder="Search..."
            mode={mode}
            delay={delay}
            onSearch={handleSearch}
          />
        </div>
      ))}
    </div>
  );
}

export default App;
