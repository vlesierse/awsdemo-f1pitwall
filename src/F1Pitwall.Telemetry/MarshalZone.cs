using System;

namespace F1Pitwall.Telemetry
{
    public class MarshalZone
    {
        public MarshalZone(ReadOnlySpan<byte> buffer)
        {
            ZoneStart = BitConverter.ToSingle(buffer.Slice(0,4));
            ZoneFlag = (ZoneFlag)buffer[4];
        }
        
        public float ZoneStart { get; }

        public ZoneFlag ZoneFlag { get; }
    }
}
