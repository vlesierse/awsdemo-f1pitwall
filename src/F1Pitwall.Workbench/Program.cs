using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using F1Pitwall.Telemetry.Server;
using F1Pitwall.Telemetry.Server.Kinesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

Console.WriteLine("F1 Pitwall Workbench");


var recorder = new TelemetrySimulator(new KinesisPacketHandler());
await recorder.Replay($"d:/zandvoort-26-05.rdf");

/*var _dynamodbClient = new AmazonDynamoDBClient();

Console.WriteLine(String.Join(',', await GetConnections("16587335118100295145")));

async Task<IEnumerable<string>> GetConnections(string session)
{
    var table = Table.LoadTable(_dynamodbClient, "f1pitwall-sessions");
    var filter = new QueryFilter("PK", QueryOperator.Equal, $"S#{session}");
    filter.AddCondition("SK", QueryOperator.BeginsWith, "CX#");
    var result = table.Query(filter);
    var documents = await result.GetNextSetAsync();
    return documents.Select(document => document["SK"].AsString().Split('#')[1]);
}*/
