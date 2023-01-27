using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace F1Pitwall.Telemetry.Server.Kinesis
{
    public class KinesisPacketHandler : IPacketHandler
    {
        private readonly AmazonKinesisClient _client;

        private uint _currentFrame = 0;
        private DateTime _sessionTime;
        private readonly List<Packet> _framePackets;
        private readonly FrameFactory _frameFactory;

        public KinesisPacketHandler()
        {
            var credentials = GetAWSCredentials();
            _client = credentials != null ? new(credentials) : new();
            _framePackets = new List<Packet>(3);
            _frameFactory = new FrameFactory();
        }

        private static AWSCredentials? GetAWSCredentials()
        {
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials("default", out var credentials))
            {
                return null;
            }
            return credentials;
        }

        public void HandleAsync(Packet packet)
        {
            if (_currentFrame == 0)
            {
                _sessionTime = DateTime.UtcNow;
            }

            // We are at the next frame. So we send the current frame to Kinesis
            if (_currentFrame != packet.Header.FrameIdentifier)
            {
                var request = new PutRecordsRequest { StreamName = Environment.GetEnvironmentVariable("KINESISSTREAM_NAME") ?? "F1Pitwall-TelemetryStream" };
                var frames = _frameFactory.CreateFrames(_framePackets, _sessionTime);

                if (frames.Any())
                {
                    var streams = new List<MemoryStream>();
                    try
                    {
                        request.Records = frames.Select(f =>
                        {
                            var json = JsonSerializer.Serialize(f, new JsonSerializerOptions
                            {
                                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });
                            var jsonData = Encoding.ASCII.GetBytes(json);
                            var dataStream = new MemoryStream(jsonData.Length);
                            streams.Add(dataStream);
                            dataStream.Write(jsonData, 0, jsonData.Length);
                            var record = new PutRecordsRequestEntry()
                            {
                                PartitionKey = f.SessionId.ToString() + f.CarIndex.ToString("D2"),
                                Data = dataStream
                            };
                            return record;
                        }).ToList();
                        var response = _client.PutRecordsAsync(request).GetAwaiter().GetResult();
                        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                        {
                            Console.WriteLine($"Failed writting frame {_currentFrame}");
                        }
                    }
                    finally
                    {
                        foreach (var stream in streams)
                        {
                            stream.Dispose();
                        }
                    }
                }
                _currentFrame = packet.Header.FrameIdentifier;
                _framePackets.Clear();
            }
            _framePackets.Add(packet);
        }

        public void OnStarted() { }
    }
}
