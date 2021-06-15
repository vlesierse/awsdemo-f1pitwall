using System;

namespace F1Pitwall.Models
{
    public class LapData
    {
        public byte LapNumber { get; set; }
        public byte Sector { get; set; }
        public float LapTime { get; set; }
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
        public byte Position { get; set; }
        public byte BestSector1Lap { get; set; }
        public byte BestSector2Lap { get; set; }
        public byte BestSector3Lap { get; set; }
        public byte GridPosition { get; set; }
    }
}
