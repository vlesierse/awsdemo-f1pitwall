using System;

namespace F1Pitwall.Telemetry
{
    public class WeatherForecastSample
    {
        public WeatherForecastSample(ReadOnlySpan<byte> buffer)
        {
            
            SessionType = (SessionType)buffer[0];
            TimeOffset = buffer[1];
            Weather = (Weather)buffer[2];
            TrackTemperature = (sbyte)buffer[3];
            AirTemperature = (sbyte)buffer[4];
        }

        public SessionType SessionType { get; }
        public byte TimeOffset { get; }
        public Weather Weather { get; }
        public sbyte TrackTemperature { get; }
        public sbyte AirTemperature { get; }
    }
}
