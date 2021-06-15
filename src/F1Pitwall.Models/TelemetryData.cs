namespace F1Pitwall.Models
{
    public class TelemetryData
    {
        public ushort Speed { get; set; }
        public float Break { get; set; }
        public float Throttle { get; set; }
        public float Gear { get; set; }
        public TyreData FL { get; set; }
        public TyreData FR { get; set; }
        public TyreData RL { get; set; }
        public TyreData RR { get; set; }
    }
}
