using System;

namespace F1Pitwall.Telemetry
{
    public record PacketHeader
    {
        public PacketHeader(ReadOnlySpan<byte> buffer)
        {
            PacketFormat = BitConverter.ToUInt16(buffer.Slice(0, 2));
            GameMajorVersion = buffer[2];
            GameMinorVersion = buffer[3];
            PacketVersion = buffer[4];
            PacketId = (PacketId)buffer[5];
            SessionId = BitConverter.ToUInt64(buffer.Slice(6, 8));
            SessionTime = BitConverter.ToSingle(buffer.Slice(14, 4));
            FrameIdentifier = BitConverter.ToUInt32(buffer.Slice(18, 4));
            PlayerCarIndex = buffer[22];
            SecondaryPlayerCarIndex = buffer[23];
        }

        public UInt16 PacketFormat { get; }
        public Byte GameMajorVersion { get; }
        public Byte GameMinorVersion { get; }
        public Byte PacketVersion { get; }
        public PacketId PacketId { get; }
        public UInt64 SessionId { get; }
        public float SessionTime { get; }
        public UInt32 FrameIdentifier { get; }
        public byte PlayerCarIndex { get;  }
        public byte SecondaryPlayerCarIndex { get; }


    }
}
