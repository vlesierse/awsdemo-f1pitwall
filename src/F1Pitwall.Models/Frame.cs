using System;

namespace F1Pitwall.Models
{
    public class Frame
    {
        public uint FrameId { get; set; }

        public DateTime Timestamp { get; set; }

        public string SessionId { get; set; }

        public float SessionTime { get; set; }

        public byte CarIndex { get; set; }

        public bool IsPlayer { get; set; }

        public LapData Lap { get; set; }

        public SessionData Session { get; set; }

        public TelemetryData Telemetry { get; set; }
    }
}
