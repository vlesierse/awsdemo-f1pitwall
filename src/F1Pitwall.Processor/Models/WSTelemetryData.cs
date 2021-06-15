using F1Pitwall.Models;

namespace F1Pitwall.Processor.Models
{
    public class WSTelemetryData : WSCarData
    {
        public ushort Speed { get; internal set; }
        public float Gear { get; internal set; }
        public float Throttle { get; internal set; }
        public float Break { get; internal set; }
        public TyreData FL { get; set; }
        public TyreData FR { get; set; }
        public TyreData RL { get; set; }
        public TyreData RR { get; set; }

        public static WSTelemetryData CreateFromFrame(Frame frame)
        {
            return new WSTelemetryData
            {
                Type = "telemetry",
                CarId = frame.CarIndex,
                FrameId = frame.FrameId,
                Speed = frame.Telemetry.Speed,
                Gear = frame.Telemetry.Gear,
                Break = frame.Telemetry.Break,
                Throttle = frame.Telemetry.Throttle,
                FL = frame.Telemetry.FL,
                FR = frame.Telemetry.FL,
                RL = frame.Telemetry.FL,
                RR = frame.Telemetry.FL,
            };
        }
    }
}
