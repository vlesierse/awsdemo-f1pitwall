
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { connectSession, joinSession } from '../store/actions';


import { Tabs, Tab } from 'react-bootstrap';
import LapData from '../components/LapData';
import TelemetryData from '../components/TelemetryData';

const Home = () => {
  const [key, setKey] = useState('home');
  const dispatch = useDispatch();
  const {connected, cars, car} = useSelector(({ session }) => session);

  useEffect(() => {
    setKey('lap')
    if (connected) {
      dispatch(joinSession('16587335118100295145'));
    } else {
      dispatch(connectSession());
    }
  }, [connected, dispatch])

  return (
    <Tabs
      id="controlled-tab-example"
      activeKey={key}
      onSelect={(k) => setKey(k)}
    >
      <Tab eventKey="lap" title="Lap">
        <LapData cars={cars} />
      </Tab>
      <Tab eventKey="telemetry" title="Telemetry">
        <TelemetryData car={car} showOverlay={key === 'telemetry'} />
      </Tab>
    </Tabs>
  );
}

export default Home