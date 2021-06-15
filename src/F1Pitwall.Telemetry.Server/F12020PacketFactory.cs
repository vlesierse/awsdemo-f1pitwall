using System;

namespace F1Pitwall.Telemetry.Server
{
    public class F12020PacketFactory : IPacketFactory
    {
        public Packet CreatePacket(byte[] data)
        {
            var header = new PacketHeader(data);
            switch(header.PacketId)
            {
                case PacketId.LapData:
                    return new PacketLapData(data);
                case PacketId.CarTelemetry:
                    return new PacketCarTelemetry(data);
                case PacketId.Session:
                    return new PacketSessionData(data);
                default:
                    Console.WriteLine($"Unknown packet {header.PacketId}");
                    return null;
            }
        }
    }
}
