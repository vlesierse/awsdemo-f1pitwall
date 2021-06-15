using System;

namespace F1Pitwall.Telemetry
{
    public struct WheelUShort
    {
        public WheelUShort(ushort rl, ushort rr, ushort fl, ushort fr)
        {
            ReerLeft = rl;
            ReerRight = rr;
            FrontLeft = fl;
            FrontRight = fr;
        }

        public WheelUShort(ReadOnlySpan<byte> buffer)
        {
            ReerLeft = BitConverter.ToUInt16(buffer.Slice(0, 2));
            ReerRight = BitConverter.ToUInt16(buffer.Slice(2, 2));
            FrontLeft = BitConverter.ToUInt16(buffer.Slice(4, 2));
            FrontRight = BitConverter.ToUInt16(buffer.Slice(6, 2));
        }

        ushort ReerLeft { get; }
        ushort ReerRight { get; }
        ushort FrontLeft { get; }
        ushort FrontRight { get; }
    }
}
