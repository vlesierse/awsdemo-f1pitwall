using F1Pitwall.Telemetry.Server;
using F1Pitwall.Telemetry.Server.Kinesis;
using System;

// UDP server port
int port = 20777;
if (args.Length > 0)
    port = int.Parse(args[0]);

Console.WriteLine($"UDP server port: {port}");

Console.WriteLine();

// Create a new UDP echo server
//var server = new TelemetryServer(new PacketRecorder(Path.Combine(Environment.CurrentDirectory, "data")), new KinesisPacketHandler());
var server = new TelemetryServer(new KinesisPacketHandler());

// Start the server
Console.Write("F1 Pitwall Telemetry Server starting...");
server.Start();
Console.WriteLine("Done!");

Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

// Perform text input
while (true)
{
    string line = Console.ReadLine();
    if (string.IsNullOrEmpty(line))
        break;

    // Restart the server
    if (line == "!")
    {
        Console.Write("Server restarting...");
        server.Restart();
        Console.WriteLine("Done!");
    }
}

// Stop the server
Console.Write("Server stopping...");
server.Stop();
Console.WriteLine("Done!");
