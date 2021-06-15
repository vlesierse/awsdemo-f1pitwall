using F1Pitwall.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F1Pitwall.Telemetry.Server.Kinesis
{
    public class FrameFactory
    {
        public IEnumerable<Frame> CreateFrames(IEnumerable<Packet> packets, DateTime sessionTime)
        {

            var header = packets.First().Header;
            var cars = packets.OfType<PacketLapData>().FirstOrDefault()?.Cars;
            var telemetry = packets.OfType<PacketCarTelemetry>().FirstOrDefault();
            var session = packets.OfType<PacketSessionData>().FirstOrDefault();
            if (cars == null && telemetry == null)
            {
                return Enumerable.Empty<Frame>();
            }
            return cars.Where(lap => lap.GridPosition != 0).Select((lap, i) => CreateFrame((byte)i, sessionTime, header, lap, telemetry.Cars[i], session));
        }

        public Frame CreateFrame(byte index, DateTime sessionTime, PacketHeader header, CarLapData lapData, CarTelemetryData telemetryData, PacketSessionData sessionData)
        {
            var frame = new Frame();
            frame.FrameId = header.FrameIdentifier;
            frame.SessionId = header.SessionId.ToString();
            frame.SessionTime = header.SessionTime;
            frame.CarIndex = index;
            frame.IsPlayer = index == header.PlayerCarIndex;
            frame.Timestamp = sessionTime.AddMilliseconds(header.FrameIdentifier * 100);

            if (sessionData != null)
            {
                frame.Session = new SessionData
                {
                    Track = (int)sessionData.Track,
                    Duration = sessionData.SessionDuration,
                    TimeLeft = sessionData.SessionTimeLeft,
                    Weather = (byte)sessionData.Weather,
                    AirTemperature = sessionData.AirTemperature,
                    TrackTemperature = sessionData.TrackTemperature
                };
            }
            if (lapData != null)
            {
                frame.Lap = new LapData
                {
                    LapNumber = lapData.CurrentLapNumber,
                    Sector = lapData.Sector,
                    Position = lapData.CarPosition,
                    TotalDistance = lapData.TotalDistance,
                    LapDistance = lapData.LapDistance,
                    LastLapTime = lapData.LastLapTime,
                    CurrentLapTime = lapData.CurrentLapTime,
                    Sector1Time = lapData.Sector1Time,
                    Sector2Time = lapData.Sector2Time,
                    BestLapTime = lapData.BestLapTime,
                    BestLapSector1Time = lapData.BestLapSector1Time,
                    BestLapSector2Time = lapData.BestLapSector2Time,
                    BestLapSector3Time = lapData.BestLapSector3Time,
                    BestSector1Lap = lapData.BestOverallSector1LapNumber,
                    BestSector1Time = lapData.BestOverallSector1Time,
                    BestSector2Lap = lapData.BestOverallSector2LapNumber,
                    BestSector2Time = lapData.BestOverallSector2Time,
                    BestSector3Lap = lapData.BestOverallSector3LapNumber,
                    BestSector3Time = lapData.BestOverallSector3Time,
                    GridPosition = lapData.GridPosition
                };
            }
            if (telemetryData != null)
            {
                frame.Telemetry = new TelemetryData
                {
                    Speed = telemetryData.Speed,
                    Break = telemetryData.Brake,
                    Throttle = telemetryData.Throttle,
                    Gear = telemetryData.Gear,
                    RR = new TyreData
                    {
                        Presure = telemetryData.TyrePresure.ReerRight,
                        InnerTemperature = telemetryData.TyreInnerTemperature.ReerRight,
                        SurfaceTemperature = telemetryData.TyreSurfaceTemperature.ReerRight
                    },
                    RL = new TyreData
                    {
                        Presure = telemetryData.TyrePresure.ReerLeft,
                        InnerTemperature = telemetryData.TyreInnerTemperature.ReerLeft,
                        SurfaceTemperature = telemetryData.TyreSurfaceTemperature.ReerLeft
                    },
                    FL = new TyreData
                    {
                        Presure = telemetryData.TyrePresure.FrontLeft,
                        InnerTemperature = telemetryData.TyreInnerTemperature.FrontLeft,
                        SurfaceTemperature = telemetryData.TyreSurfaceTemperature.FrontLeft
                    },
                    FR = new TyreData
                    {
                        Presure = telemetryData.TyrePresure.FrontRight,
                        InnerTemperature = telemetryData.TyreInnerTemperature.FrontRight,
                        SurfaceTemperature = telemetryData.TyreSurfaceTemperature.FrontRight
                    },
                };
            }
            return frame;
        }
    }
}
