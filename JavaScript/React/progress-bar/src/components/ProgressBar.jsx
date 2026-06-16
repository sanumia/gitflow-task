import {useState, useEffect} from 'react'
const ProgressBar = () => {
    const [filled, setFilled] = useState(0)
    const [loading, setLoading] = useState(false)

    useEffect(() =>{
        if(filled < 100 && loading){
            setTimeout(() => setFilled(prev => prev+=5),50)
        }
    }, [filled, loading])
    return (
        <div style={{ 
            display: 'flex', 
            flexDirection: 'column', 
            alignItems: 'center',   // выравнивание по центру по горизонтали
            gap: '10px',            // отступы между элементами
            margin: 50 }}>
        <div 
            className="progress-bar" 
            style={{ 
                width: '300px', 
                height: '20px',
                margin: 50,
                backgroundColor: '#f0f0f0',
                borderRadius: 40,
                overflow: 'hidden'
            }}  
            >
            <div style={{
                height: "100%",
                width: `${filled}%`,
                backgroundColor: "pink",
                transition: "width 0.5s"
            }} />
        </div>
            <span className="progress-bar__percentage">
                {filled} %
            </span>
            <button className="btn text-white" onClick={() => { setLoading(true) }}> Start </button>
        </div>

    )
}
export default ProgressBar