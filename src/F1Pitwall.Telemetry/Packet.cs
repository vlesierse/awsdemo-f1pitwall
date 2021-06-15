using System;

namespace F1Pitwall.Telemetry
{
    public record Packet
    {
        public Packet(ReadOnlyMemory<byte> data)
        {
            Header = new PacketHeader(data.Span.Slice(0, 24));
            RawData = data;
        }

        public PacketHeader Header { get; }

        public ReadOnlyMemory<byte> RawData { get;  }

        public const int BufferSize = 8192;
    }
}
