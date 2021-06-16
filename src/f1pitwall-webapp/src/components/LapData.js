import { Table } from 'react-bootstrap';

const AsTimeString = (time) => {
  return `${Math.floor(time/60)}:${(time - ((time/60).toFixed(0) * 60)).toFixed(3)}`
}

const TextColor = (currentTime, lastTime, overallTime) => {
  /*if (currentTime < overallTime) {
    return 'purple';
  }
  if (currentTime < lastTime) {
    return 'green';
  }*/
  return 'white'
}

const LapData = ({cars}) => {
  var sorted = [...cars].filter(e => e !== undefined);
  sorted.sort((a, b) =>  {
    var result = b.lapNumber - a.lapNumber;
    return result === 0 ? (b.lapDistance) - (a.lapDistance) : result;
  });
  return (
    <Table striped bordered hover size="sm" variant="dark">
      <thead>
        <tr>
          <th>Driver</th>
          <th>Lap</th>
          <th>Position</th>
          <th>Time</th>
          <th>Gap</th>
          <th>Interval</th>
          <th>Sector 1</th>
          <th>Sector 2</th>
        </tr>
      </thead>
      <tbody>
      {sorted.map((e, i) => (
        <tr key={i}>
          <td>{e.carId}</td>
          <td>{e.lapNumber}</td>
          <td>{e.gridPosition - e.position}</td>
          <td style={{ color: TextColor(e.lastLapTime, e.bestSector1Time + e.bestSector2Time + e.bestSector3Time, e.bestLapTime)}}>{AsTimeString(e.lastLapTime)}</td>
          <td>{i !== 0 ? (sorted[0].currentLapTime - e.currentLapTime).toFixed(3) : ''}</td>
          <td>{i !== 0 ? (sorted[i-1].currentLapTime - e.currentLapTime).toFixed(3) : ''}</td>
          <td style={{ color: TextColor(e.sector1Time, e.bestSector1Time, e.bestLapSector1Time)}}>{e.sector1Time !== 0 ? (e.sector1Time/1000).toFixed(3) : ''}</td>
          <td style={{ color: TextColor(e.sector2Time, e.bestSector2Time, e.bestLapSector2Time)}}>{e.sector2Time !== 0 ? (e.sector2Time/1000).toFixed(3) : ''}</td>
        </tr>
      ))}
      </tbody>
    </Table>
  )
}

export default LapData;