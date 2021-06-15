using System;
using System.Numerics;

namespace F1Pitwall.Telemetry
{
    public record CarMotionData
    {
        public CarMotionData(ReadOnlySpan<byte> buffer)
        {
            WorldPosition = new Vector3(BitConverter.ToSingle(buffer.Slice(0, 4)), BitConverter.ToSingle(buffer.Slice(4, 4)), BitConverter.ToSingle(buffer.Slice(8, 4)));
            WorldVelocity = new Vector3(BitConverter.ToSingle(buffer.Slice(12, 4)), BitConverter.ToSingle(buffer.Slice(16, 4)), BitConverter.ToSingle(buffer.Slice(20, 4)));
            WorldDirectionForward = new Direction(buffer.Slice(24, 6));
            WorldDirectionRight = new Direction(buffer.Slice(30, 6));
            GForceLateral = BitConverter.ToSingle(buffer.Slice(36, 4));
            GForceLongitudinal = BitConverter.ToSingle(buffer.Slice(40, 4));
            GForceVertical = BitConverter.ToSingle(buffer.Slice(44, 4));
            Yaw = BitConverter.ToSingle(buffer.Slice(48, 4));
            Pitch = BitConverter.ToSingle(buffer.Slice(52, 4));
            Roll = BitConverter.ToSingle(buffer.Slice(56, 4));
        }

        public Vector3 WorldPosition { get; private set; }

        public Vector3 WorldVelocity { get; private set; }

        public Direction WorldDirectionForward { get; private set; }

        public Direction WorldDirectionRight { get; private set; }

        public float GForceLateral { get; }
        public float GForceLongitudinal { get; }
        public float GForceVertical { get; }
        public float Yaw { get; }
        public float Pitch { get; }
        public float Roll { get; }
    }
}
