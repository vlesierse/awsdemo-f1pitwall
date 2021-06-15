using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace F1Pitwall.Telemetry
{
    public class TelemetryRecorder
    {

        public async Task Replay(string file, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                
                uint currentFrame = 0;
                var buffer = new byte[8192];
                while (await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken) != -1)
                {
                    var data = buffer.AsMemory();
                    var header = new PacketHeader(data.Span);
                    if (header.FrameIdentifier != currentFrame)
                    {
                        currentFrame = header.FrameIdentifier;
                        await Task.Delay(50);
                    }
                    switch (header.PacketId)
                    {
                        case PacketId.Session:
                            var session = new PacketSessionData(data);
                            Console.WriteLine($"Session(Track={session.Track},Type={session.SessionType},Weather={session.Weather})");
                            break;
                        case PacketId.Motion:
                            var motion = new PacketMotionData(data);
                            Console.WriteLine($"Motion()");
                            break;
                        case PacketId.LapData:
                            var lap = new PacketLapData(data);
                            Console.WriteLine($"Lap()");
                            break;
                        case PacketId.CarTelemetry:
                            var telemetry = new PacketCarTelemetry(data);
                            Console.WriteLine($"Telemetry()");
                            break;
                        default:
                            Console.WriteLine($"Packet(ID={header.PacketId},Version={header.PacketVersion},GameVersion={header.GameMajorVersion}.{header.GameMinorVersion})");
                            break;
                    }
                }
            }
        }

        public Task Record(string file)
        {
            return Task.CompletedTask;
        }
    }
}
