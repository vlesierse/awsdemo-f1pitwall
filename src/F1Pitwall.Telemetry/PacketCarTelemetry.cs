using System;
using System.Linq;

namespace F1Pitwall.Telemetry
{
    public record PacketCarTelemetry : Packet
    {
        public PacketCarTelemetry(ReadOnlyMemory<byte> data) : base(data)
        {
            Cars = Enumerable.Range(0, 22).Select(i => new CarTelemetryData(data.Span.Slice(24 + (i * 58), 58))).ToArray();
        }

        public CarTelemetryData[] Cars { get; }
    }
}
