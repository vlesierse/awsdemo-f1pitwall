using System;

namespace F1Pitwall.Telemetry
{
    public struct WheelByte
    {
        public WheelByte(byte rl, byte rr, byte fl, byte fr)
        {
            ReerLeft = rl;
            ReerRight = rr;
            FrontLeft = fl;
            FrontRight = fr;
        }

        public WheelByte(ReadOnlySpan<byte> buffer)
        {
            ReerLeft = buffer[0];
            ReerRight = buffer[1];
            FrontLeft = buffer[2];
            FrontRight = buffer[3];
        }

        public byte ReerLeft { get; }
        public byte ReerRight { get; }
        public byte FrontLeft { get; }
        public byte FrontRight { get; }
    }
}
