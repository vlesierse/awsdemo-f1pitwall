using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace F1Pitwall.Processor
{
    public class SessionContext
    {
        private AmazonDynamoDBClient _dynamodbClient;
        private List<string> _connections;
        private static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        public SessionContext(AmazonDynamoDBClient dynamodbClient, string session)
        {
            Session = session;
            _dynamodbClient = dynamodbClient;
        }

        public string Session { get; }

        public async Task<IEnumerable<string>> GetConnections()
        {
            await Semaphore.WaitAsync();
            try
            {
                if (_connections == null)
                {

                    var table = Table.LoadTable(_dynamodbClient, Environment.GetEnvironmentVariable("WEBSOCKETAPI_TABLE") ?? "F1Pitwall-Sessions");
                    var filter = new QueryFilter("PK", QueryOperator.Equal, $"S#{Session}");
                    filter.AddCondition("SK", QueryOperator.BeginsWith, "CX#");
                    var result = table.Query(filter);
                    var documents = await result.GetNextSetAsync();
                    _connections = new List<string>(documents.Select(document => document["SK"].AsString().Split('#')[1]));
                    LambdaLogger.Log($"Discovered {_connections.Count} connections to session {Session}");
                }
                return _connections;
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public async Task DeleteConnection(string connection)
        {
            await Semaphore.WaitAsync();
            try
            {
                await _dynamodbClient.DeleteItemAsync(new DeleteItemRequest
                {
                    TableName = "f1pitwall-sessions",
                    Key = new Dictionary<string, AttributeValue> {
                        { "PK", new AttributeValue { S = $"S#{Session}" } },
                        { "SK", new AttributeValue { S = $"CX#{connection}" } }
                    }
                });
                _connections.Remove(connection);
                LambdaLogger.Log($"Removed {connection} from session {Session}");
            }
            catch (AmazonDynamoDBException ex)
            {
                LambdaLogger.Log($"Failed removing {connection} from session {Session}: {ex.Message}");
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}
