namespace F1Pitwall.Telemetry
{
    public enum PacketId : byte
    {
        Motion = 0,
        Session = 1,
        LapData = 2,
        Event = 3,
        Participants = 4,
        CarSetups = 5,
        CarTelemetry = 6,
        CarStatus = 7,
        FinalClassification = 8,
        LobbyInfo = 9
    }
}
