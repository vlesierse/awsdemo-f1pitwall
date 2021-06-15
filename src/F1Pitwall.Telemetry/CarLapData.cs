using System;

namespace F1Pitwall.Telemetry
{
    public record CarLapData
    {
        public CarLapData(ReadOnlySpan<byte> buffer)
        {
            LastLapTime = BitConverter.ToSingle(buffer.Slice(0, 4));
            CurrentLapTime = BitConverter.ToSingle(buffer.Slice(4, 4));

            Sector1Time = BitConverter.ToUInt16(buffer.Slice(8, 2));
            Sector2Time = BitConverter.ToUInt16(buffer.Slice(10, 2));
            BestLapTime = BitConverter.ToSingle(buffer.Slice(12, 4));
            BestLapNumber = buffer[16];
            BestLapSector1Time = BitConverter.ToUInt16(buffer.Slice(17, 2));
            BestLapSector2Time = BitConverter.ToUInt16(buffer.Slice(19, 2));
            BestLapSector3Time = BitConverter.ToUInt16(buffer.Slice(21, 2));
            BestOverallSector1Time = BitConverter.ToUInt16(buffer.Slice(23, 2));
            BestOverallSector2LapNumber = buffer[25];
            BestOverallSector2Time = BitConverter.ToUInt16(buffer.Slice(26, 2));
            BestOverallSector2LapNumber = buffer[28];
            BestOverallSector3Time = BitConverter.ToUInt16(buffer.Slice(29, 2));
            BestOverallSector3LapNumber = buffer[31];

            LapDistance = BitConverter.ToSingle(buffer.Slice(32, 4));
            TotalDistance = BitConverter.ToSingle(buffer.Slice(36, 4));
            SafetyCarDelta = BitConverter.ToSingle(buffer.Slice(40, 4));
            CarPosition = buffer[44];
            CurrentLapNumber = buffer[45];
            PitStatus = (PitStatus)buffer[46];
            Sector = buffer[47];
            CurrentLapInvalid = buffer[48] != 0;
            TimePenalties = buffer[49];
            GridPosition = buffer[50];
            DriverStatus = (DriverStatus)buffer[51];
            ResultStatus = (ResultStatus)buffer[52];
        }

        public float LastLapTime { get; }

        public float CurrentLapTime { get; }

        public ushort Sector1Time { get; }

        public ushort Sector2Time { get; }

        public float BestLapTime { get; }

        public byte BestLapNumber { get; }
        public ushort BestLapSector1Time { get; }
        public ushort BestLapSector2Time { get; }
        public ushort BestLapSector3Time { get; }
        public ushort BestOverallSector1Time { get; }
        public byte BestOverallSector1LapNumber { get; }
        public ushort BestOverallSector2Time { get; }
        public byte BestOverallSector2LapNumber { get; }
        public ushort BestOverallSector3Time { get; }
        public byte BestOverallSector3LapNumber { get; }

        public float LapDistance { get; }
        public float TotalDistance { get; }
        public float SafetyCarDelta { get; }
        public byte CarPosition { get; }
        public byte CurrentLapNumber { get; }
        public PitStatus PitStatus { get; }
        public byte Sector { get; }
        public bool CurrentLapInvalid { get; }
        public byte TimePenalties { get; }
        public byte GridPosition { get; }
        public DriverStatus DriverStatus { get; }
        public ResultStatus ResultStatus { get; }
    }
}
