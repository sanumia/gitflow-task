import Timer from './Timer';
import Button from './Button';
import {useState} from 'react';

function TimerForm() {
    
    const [isRunning, setIsRunning] = useState(false);
    const [resetKey, setResetKey] = useState(0);

    const handleStart = () => setIsRunning(true);

    const handleStop = () => setIsRunning(false);

    const handleReset = () => {
        setIsRunning(false);
        setResetKey(prev => prev + 1);
    };

    return (
        <div>
            <Timer
                key={resetKey} // reset by remounting
                settings={{ duration: 60 }} // example: 60 seconds
                onComplete={() => alert('Time is up!')}
                isRunning={isRunning} 
            >
                {(hours, minutes, seconds) => (
                <span>
                    {String(hours).padStart(2, '0')}:
                    {String(minutes).padStart(2, '0')}:
                    {String(seconds).padStart(2, '0')}
                </span>
            )}
            </Timer>
            <div>
                <Button onClick={handleStart}>Start</Button>
                <Button onClick={handleStop}>Stop</Button>
                <Button onClick={handleReset}>Reset</Button>
            </div>
        </div>
    );
}

export default TimerForm;