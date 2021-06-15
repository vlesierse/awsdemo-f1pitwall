using Microsoft.Toolkit.HighPerformance;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace F1Pitwall.Telemetry.Server
{
    public class PacketRecorder : IPacketHandler, IDisposable
    {

        private readonly string _folder;
        private readonly ConcurrentDictionary<ulong, Stream> _streams;


        public PacketRecorder(string folder)
        {
            _folder = folder;
            _streams = new ConcurrentDictionary<ulong, Stream>();
        }

        public void Dispose()
        {
            foreach(var stream in _streams.Values)
            {
                stream.Dispose();
            }
        }

        public void HandleAsync(Packet packet)
        {
            var stream = _streams.GetOrAdd(packet.Header.SessionId, i => CreateSessionStream(i));
            Console.WriteLine($"Write packet {packet.Header.PacketId} [{packet.Header.FrameIdentifier}]");
            using (var dataStream = packet.RawData.AsStream())
            {
                dataStream.CopyTo(stream);
            }
        }

        public void OnStarted()
        {
            Directory.CreateDirectory(_folder);
        }

        private Stream CreateSessionStream(ulong sessionId)
        {
            Console.WriteLine($"Session {sessionId} Started.");
            return new FileStream(Path.Combine(_folder, $"session-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}-{sessionId}.rdf"), FileMode.Append);
        }
    }
}
