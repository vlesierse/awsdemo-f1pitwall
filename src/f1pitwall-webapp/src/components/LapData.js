import { Table } from 'react-bootstrap';

const AsTimeString = (time) => {
  return `${Math.floor(time/60)}:${(time - ((time/60).toFixed(0) * 60)).toFixed(3)}`
}

const LapData = ({cars}) => {
  var sorted = [...cars].filter(e => e !== undefined);
  sorted.sort((a, b) => a.position - b.position);
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
          <td>{AsTimeString(e.lastLapTime)}</td>
          <td>{i !== 0 ? (sorted[0].currentLapTime - e.currentLapTime).toFixed(3) : ''}</td>
          <td>{i !== 0 ? (sorted[i-1].currentLapTime - e.currentLapTime).toFixed(3) : ''}</td>
          <td>{e.sector1Time.toFixed(3)}</td>
          <td>{e.sector2Time.toFixed(3)}</td>
        </tr>
      ))}
      </tbody>
    </Table>
  )
}

export default LapData;