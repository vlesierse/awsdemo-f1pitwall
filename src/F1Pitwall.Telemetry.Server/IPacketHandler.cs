using System.Threading.Tasks;

namespace F1Pitwall.Telemetry.Server
{
    public interface IPacketHandler
    {
        void HandleAsync(Packet packet);
        void OnStarted();
    }
}
