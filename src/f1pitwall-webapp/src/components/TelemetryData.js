import { useRef } from 'react';
import imageCarTop from '../assets/images/car-top.png'

import { Container, Image, Row, Col, Overlay, ProgressBar, Table } from 'react-bootstrap';

const TyreData = ({ tyre }) => {
  if (!tyre) return <div />;

  return (
    <Table striped bordered responsive variant="dark">
      <tbody>
        <tr>
          <td>Presure</td>
          <td>{tyre.presure}</td>
        </tr>
        <tr>
          <td>Surface Temp.</td>
          <td>{tyre.surfaceTemperature}</td>
        </tr>
        <tr>
          <td>Inner Temp.</td>
          <td>{tyre.innerTemperature}</td>
        </tr>
      </tbody>
    </Table>
  )
}

const TelemetryData = ({ car, showOverlay }) => {
  const target = useRef(null);

  if (!car) return <div />;

  return (
    <>
      <div ref={target}></div>
      <Image src={imageCarTop} fluid />
      <Table striped bordered hover responsive variant="dark">
        <tbody>
          <tr>
            <td>Gear</td>
            <td>{car.gear}</td>
          </tr>
          <tr>
            <td>Throttle</td>
            <td>
              <ProgressBar now={car.throttle * 100} />
            </td>
          </tr>
          <tr>
            <td>Break</td>
            <td>
              <ProgressBar now={car.break * 100} />
            </td>
          </tr>
        </tbody>
      </Table>
      <Overlay target={target} placement="bottom" show={showOverlay}>
        <Container fluid>
          <Row style={{ height: '50px' }}>
            <Col></Col>
          </Row>
          <Row style={{ height: '150px' }}>
            <Col lg={{ span: 4 }}></Col>
            <Col style={{ backgroundColor: '#FFFFFFCC', border: '1px solid black' }}>
              <TyreData tyre={car.rl} />
            </Col>
            <Col lg={{ span: 2 }}></Col>
            <Col style={{ backgroundColor: '#FFFFFFCC', border: '1px solid black' }}>
              <TyreData tyre={car.fl} />
            </Col>
            <Col lg={{ span: 4 }}></Col>
          </Row>
          <Row style={{ height: '20px' }}>
            <Col></Col>
          </Row>
          <Row style={{ height: '150px' }}>
            <Col lg={{ span: 4 }}></Col>
            <Col style={{ backgroundColor: '#FFFFFFCC', border: '1px solid black' }}>
              <TyreData tyre={car.rr} />
            </Col>
            <Col lg={{ span: 2 }}></Col>
            <Col style={{ backgroundColor: '#FFFFFFCC', border: '1px solid black' }}>
              <TyreData tyre={car.fr} />
            </Col>
            <Col lg={{ span: 4 }}></Col>
          </Row>
        </Container>
      </Overlay>
    </>
  )
}

export default TelemetryData;