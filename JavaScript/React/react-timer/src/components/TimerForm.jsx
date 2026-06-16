import Timer from "./Timer"
import Button from "./Button"
import {useState, useEffect, useRef} from 'react'

const TimerForm = () => {
    const [seconds, setSeconds] = useState(0)
    
    const [isRunning, setIsRunning] = useState(false)

    const intervalRef = useRef(null) 

    useEffect(() =>{
        if(isRunning){
            intervalRef.current = setInterval(() => {
                setSeconds(prev => prev + 1);
            }, 1000);
        }
        else{
            if(intervalRef.current){
                clearInterval(intervalRef.current)
            }
        }

        return () => clearInterval(intervalRef.current);
    }, [isRunning])

    const handleStart = () => setIsRunning(true)

    const handleStop = () => setIsRunning(false)

    const handleReset = () => {
        setIsRunning(false)
        setSeconds(0)
    }

    return (
        <div>
            <Timer time = {seconds}/>
            <div>
                <Button onClick={handleStart}> Start </Button>
                <Button onClick={handleStop}> Stop </Button>
                <Button onClick={handleReset}> Reset </Button>
            </div>
        </div>
    )
}

export default TimerForm