import Timer from './Timer';
import Button from './Button';
import {useState, useCallback} from 'react';
import {SECONDS_PER_MINUTE} from "../constants/timeConstants";


function TimerForm() {
    const [isRunning, setIsRunning] = useState(false);
    const [resetKey, setResetKey] = useState(0);

    const handleStart = useCallback(() => setIsRunning(true));

    const handleStop = useCallback(() => setIsRunning(false));

    const handleReset = useCallback(() => {
        setIsRunning(false);
        setResetKey(prev => prev + 1);
    });

    return (
        <div>
            <Timer
                key={resetKey}
                settings={{ duration: SECONDS_PER_MINUTE }}
                onComplete={() => alert('Time is up!')}
                isRunning={isRunning} 
            >
            {
                (hours, minutes, seconds) => (
                    <span>
                        {String(hours).padStart(2, '0')}:
                        {String(minutes).padStart(2, '0')}:
                        {String(seconds).padStart(2, '0')}
                    </span>
                )
            }
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