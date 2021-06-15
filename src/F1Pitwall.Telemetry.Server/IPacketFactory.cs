namespace F1Pitwall.Telemetry.Server
{
    public interface IPacketFactory
    {
        Packet CreatePacket(byte[] data);
    }
}
