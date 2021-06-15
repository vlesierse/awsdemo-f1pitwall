using System;
using System.Linq;

namespace F1Pitwall.Telemetry
{
    public record PacketLapData : Packet
    {
        public PacketLapData(ReadOnlyMemory<byte> data) : base(data)
        {
            Cars = Enumerable.Range(0, 22).Select(i => new CarLapData(data.Span.Slice(24 + (i * 53), 53))).ToArray();
        }

        public CarLapData[] Cars { get; }
    }
}
