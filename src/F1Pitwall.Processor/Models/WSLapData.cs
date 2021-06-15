using F1Pitwall.Models;

namespace F1Pitwall.Processor.Models
{
    public class WSLapData : WSCarData
    {
        public byte LapNumber { get; set; }
        public byte Sector { get; set; }
        public float LapTime { get; set; }
        public byte Position { get; set; }
        public byte GridPosition { get; set; }
        public ushort Sector1Time { get; set; }
        public ushort Sector2Time { get; set; }
        public float LapDistance { get; set; }
        public float CurrentLapTime { get; set; }
        public float TotalDistance { get; set; }
        public float LastLapTime { get; set; }
        public float BestLapTime { get; set; }
        public ushort BestLapSector1Time { get; set; }
        public ushort BestLapSector2Time { get; set; }
        public ushort BestLapSector3Time { get; set; }
        public ushort BestSector1Time { get; set; }
        public ushort BestSector2Time { get; set; }
        public ushort BestSector3Time { get; set; }
        public byte BestSector1Lap { get; set; }
        public byte BestSector2Lap { get; set; }
        public byte BestSector3Lap { get; set; }

        public static WSLapData CreateFromFrame(Frame frame)
        {
            return new WSLapData
            {
                Type = "lap",
                CarId = frame.CarIndex,
                FrameId = frame.FrameId,
                LapNumber = frame.Lap.LapNumber,
                LapTime = frame.Lap.LapTime,
                LapDistance = frame.Lap.LapDistance,
                Position = frame.Lap.Position,
                GridPosition = frame.Lap.GridPosition,
                Sector = frame.Lap.Sector,
                Sector1Time = frame.Lap.Sector1Time,
                Sector2Time = frame.Lap.Sector2Time,
                LastLapTime = frame.Lap.LastLapTime,
                CurrentLapTime = frame.Lap.CurrentLapTime,
                BestLapTime = frame.Lap.BestLapTime,
                BestLapSector1Time = frame.Lap.BestLapSector1Time,
                BestLapSector2Time = frame.Lap.BestLapSector2Time,
                BestLapSector3Time = frame.Lap.BestLapSector3Time
            };
        }
    }
}
