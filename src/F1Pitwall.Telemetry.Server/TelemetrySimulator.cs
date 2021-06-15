using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace F1Pitwall.Telemetry.Server
{
    public class TelemetrySimulator
    {
        private readonly IPacketFactory _factory;
        private readonly IEnumerable<IPacketHandler> _handlers;

        public TelemetrySimulator(IPacketFactory factory, IEnumerable<IPacketHandler> handlers)
        {
            _factory = factory;
            _handlers = handlers;
        }
        public TelemetrySimulator(params IPacketHandler[] handlers)
            : this(new F12020PacketFactory(), handlers)
        { }

        public async Task Replay(string file, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                uint currentFrame = 0;
                var buffer = new byte[Packet.BufferSize];
                while (await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken) != -1)
                {
                    var packet = _factory.CreatePacket(buffer);
                    if (packet != null)
                    {
                        if (packet.Header.FrameIdentifier != currentFrame)
                        {
                            currentFrame = packet.Header.FrameIdentifier;
                            await Task.Delay(100);
                        }
                        Parallel.ForEach(_handlers, h => h.HandleAsync(packet));
                    }
                }
            }
        }
    }
}
