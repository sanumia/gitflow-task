const Timer = (props) => {
    const {
        time,
    } = props

    const formatTime = (totalSecs) => {
        const mins = Math.floor(totalSecs / 60)
        const secs = totalSecs % 60
        
        return `${mins.toString()} : ${secs.toString()}`
    }

    return (
        <>
        {formatTime(time)}
        </>
    )
}

export default Timer