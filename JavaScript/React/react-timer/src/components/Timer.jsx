import {useState, useEffect} from 'react';
import { SECONDS_PER_HOUR, SECONDS_PER_MINUTE } from "../constants/timeConstants";


function Timer(props) {
    const {
        id,
        settings,
        onComplete,
        children,
        isRunning,
    } = props;


    const [remaining, setRemaining] = useState(settings.duration);

    useEffect(() =>{
        if (remaining < 0) {
            onComplete?.();

            return;
        }

        if (!isRunning) {
            return;
        }

        const interval = setInterval(() =>{
            setRemaining(prev => prev - 1);
        }, 1000);

        return () => clearInterval(interval);
    }, [remaining, onComplete, isRunning]);
    
    const hours = Math.floor(remaining / SECONDS_PER_HOUR);
    const minutes = Math.floor((remaining % SECONDS_PER_HOUR) / SECONDS_PER_MINUTE);
    const seconds = remaining % SECONDS_PER_MINUTE;

    return (
        <div id={id}>
            {children(hours, minutes, seconds)}
        </div>
    );
}

export default Timer;