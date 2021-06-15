using System;

namespace F1Pitwall.Telemetry
{
    public record CarTelemetryData
    {
        public CarTelemetryData(ReadOnlySpan<byte> buffer)
        {
            Speed = BitConverter.ToUInt16(buffer.Slice(0, 2));
            Throttle = BitConverter.ToSingle(buffer.Slice(2, 4));
            Steer = BitConverter.ToSingle(buffer.Slice(6, 4));
            Brake = BitConverter.ToSingle(buffer.Slice(10, 4));
            Clutch = buffer[14];
            Gear = (sbyte)buffer[15];
            EngineRPM = BitConverter.ToUInt16(buffer.Slice(16, 2));
            DRS = buffer[18] != 0;
            ReverseLights = buffer[19];
            BrakeTemperature = new WheelUShort(buffer.Slice(20, 8));
            TyreSurfaceTemperature = new WheelByte(buffer.Slice(28, 4));
            TyreInnerTemperature = new WheelByte(buffer.Slice(32, 4));
            EngineTemperature = BitConverter.ToUInt16(buffer.Slice(36, 2));
            TyrePresure = new WheelFloat(buffer.Slice(38, 16));
            SurfaceType = new WheelSurfaceType(buffer.Slice(54, 4));
        }

        public ushort Speed { get; }
        public float Throttle { get; }
        public float Steer { get; }
        public float Brake { get; }
        public byte Clutch { get; }
        public sbyte Gear { get; }
        public ushort EngineRPM { get; }
        public bool DRS { get; }
        public byte ReverseLights { get; }
        public WheelUShort BrakeTemperature { get; }
        public WheelByte TyreSurfaceTemperature { get; }
        public WheelByte TyreInnerTemperature { get; }
        public ushort EngineTemperature { get; }
        public WheelFloat TyrePresure { get; }
        public WheelSurfaceType SurfaceType { get; }
    }
}
