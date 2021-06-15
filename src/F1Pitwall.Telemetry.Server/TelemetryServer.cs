using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace F1Pitwall.Telemetry.Server
{
    public class TelemetryServer : UdpServer
    {
        private readonly IPacketFactory _factory;
        private readonly IEnumerable<IPacketHandler> _handlers;
        private readonly TelemetryServerOptions _options;

        public TelemetryServer(IPacketFactory factory, IEnumerable<IPacketHandler> handlers, TelemetryServerOptions options)
            : base(IPAddress.Parse(options.IPAddress), options.Port)
        {
            _options = options;
            _factory = factory;
            _handlers = handlers;
        }

        public TelemetryServer(params IPacketHandler[] handlers)
            : this(new F12020PacketFactory(), handlers, new TelemetryServerOptions())
        { }

        protected override void OnStarted()
        {
            Parallel.ForEach(_handlers, h => h.OnStarted());
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            var packet = _factory.CreatePacket(buffer);
            if (packet == null)
            {
                ReceiveAsync();
                return;
            }
            Console.WriteLine($"Packet {packet.Header.PacketId} received");
            Parallel.ForEach(_handlers, h => h.HandleAsync(packet));
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Echo UDP server caught an error with code {error}");
        }

        protected override void Dispose(bool disposingManagedResources)
        {
            base.Dispose(disposingManagedResources);
            if (disposingManagedResources)
            {
                Parallel.ForEach(_handlers.OfType<IDisposable>(), h => h.Dispose());
            }
        }
    }
}
