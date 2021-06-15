using System;

namespace F1Pitwall.Telemetry
{
    public struct Direction
    {
        public Direction(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Direction(ReadOnlySpan<byte> buffer)
        {
            X = BitConverter.ToInt16(buffer.Slice(0, 2));
            Y = BitConverter.ToInt16(buffer.Slice(2, 2));
            Z = BitConverter.ToInt16(buffer.Slice(4, 2));
        }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }
    }
}
