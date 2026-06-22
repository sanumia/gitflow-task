import {useState, useEffect, useCallback} from 'react'
import './ProgressBar.css';

const ProgressBar = () => {
    const PROGRESS_COMPLETE = 100;


    const [isFilled, setIsFilled] = useState(0)
    const [loading, setLoading] = useState(false)

    useEffect(() => {
        if(isFilled < PROGRESS_COMPLETE && loading){
            setTimeout(() => setIsFilled(prev => prev + 5), 50)
        }
    }, [isFilled, loading])

    const handleStart = useCallback(() => {
        setLoading(true);
    }, []);


    return (
        <div className="progress-bar-container">
            <div className="progress-bar">
                <div
                    className="progress-bar-fill"
                    style={{ width: `${isFilled}%` }}
                />
            </div>
            <span className="progress-bar__percentage">
                {isFilled} %
            </span>
            <button 
                className="btn text-white" 
                onClick={handleStart}
            > 
                Start 
            </button>
        </div>
    )
}

export default ProgressBar