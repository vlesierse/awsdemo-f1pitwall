namespace F1Pitwall.Telemetry.Server
{
    public class TelemetryServerOptions
    {
        public int Port { get; set; } = 20777;

        public string IPAddress { get; set; } = "0.0.0.0";
    }
}
