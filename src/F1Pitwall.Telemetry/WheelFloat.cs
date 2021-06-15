using System;

namespace F1Pitwall.Telemetry
{
    public struct WheelFloat
    {
        public WheelFloat(float rl, float rr, float fl, float fr)
        {
            ReerLeft = rl;
            ReerRight = rr;
            FrontLeft = fl;
            FrontRight = fr;
        }

        public WheelFloat(ReadOnlySpan<byte> buffer)
        {
            ReerLeft = BitConverter.ToSingle(buffer.Slice(0, 4));
            ReerRight = BitConverter.ToSingle(buffer.Slice(4, 4));
            FrontLeft = BitConverter.ToSingle(buffer.Slice(8, 4));
            FrontRight = BitConverter.ToSingle(buffer.Slice(12, 4));
        }

        public float ReerLeft { get; }
        public float ReerRight { get; }
        public float FrontLeft { get; }
        public float FrontRight { get; }
    }
}
