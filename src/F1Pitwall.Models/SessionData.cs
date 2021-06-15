namespace F1Pitwall.Models
{
    public class SessionData
    {
        public int Track { get; set; }

        public ushort Duration { get; set; }

        public ushort TimeLeft { get; set; }

        public byte Weather { get; set; }

        public sbyte AirTemperature { get; set; }

        public sbyte TrackTemperature { get; set; }
    }
}
