using System.Threading.Tasks;

namespace F1Pitwall.Telemetry
{
    public interface IPacketHandler
    {
        Task Handle(Packet packet);
    }

    public interface IPacketHandler<T> : IPacketHandler where T : Packet
    {
        Task Handle(T packet);
    }
}
