using System;
using System.Linq;

namespace F1Pitwall.Telemetry
{
    public record PacketSessionData : Packet
    {
        public PacketSessionData(ReadOnlyMemory<byte> data) : base(data)
        {
            var buffer = data.Span;
            Weather = (Weather)buffer[24];
            TrackTemperature = (sbyte)buffer[25];
            AirTemperature = (sbyte)buffer[26];
            TotalLaps = buffer[27];
            TrackLength = BitConverter.ToUInt16(buffer.Slice(28, 2));
            SessionType = (SessionType)buffer[30];
            Track = (Track)buffer[31];
            Formula = (Formula)buffer[32];
            SessionTimeLeft = BitConverter.ToUInt16(buffer.Slice(33, 2));
            SessionDuration = BitConverter.ToUInt16(buffer.Slice(35, 2));
            PitSpeedLimit = buffer[37];
            GamePaused = buffer[38] != 0;
            IsSpectating = buffer[39] != 0;
            SpectatorCarIndex = buffer[40];
            SLIProNativeActive = buffer[41] != 0;

            MarshalZones = Enumerable.Range(0, buffer[42]).Select(i => new MarshalZone(data.Span.Slice(43 + (i * 5), 5))).ToArray();

            SafetyCar = (SafetyCar)buffer[148];
            NetworkGame = (NetworkGame)buffer[149];
            WeatherForecastSamples = Enumerable.Range(0, buffer[150]).Select(i => new WeatherForecastSample(data.Span.Slice(151 + (i * 5), 5))).ToArray();
        }

        public Weather Weather { get; }
        public sbyte TrackTemperature { get; }
        public sbyte AirTemperature { get; }
        public byte TotalLaps { get; }
        public UInt16 TrackLength { get; }
        public SessionType SessionType { get; }
        public Track Track { get; }
        public Formula Formula { get; }
        public UInt16 SessionTimeLeft { get; }
        public UInt16 SessionDuration { get; }

        public byte PitSpeedLimit { get; }
        public bool GamePaused { get; }
        public bool IsSpectating { get; }
        public byte SpectatorCarIndex { get; }
        public bool SLIProNativeActive { get; }

        public MarshalZone[] MarshalZones { get; }

        public SafetyCar SafetyCar { get; }
        public NetworkGame NetworkGame { get; }

        public WeatherForecastSample[] WeatherForecastSamples { get; }
    }
}
