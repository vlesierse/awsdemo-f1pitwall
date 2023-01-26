using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using F1Pitwall.Models;
using F1Pitwall.Processor.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace F1Pitwall.Processor.Handlers
{
    public class WebSocketFrameHandler : IFrameHandler
    {
        private IAmazonApiGatewayManagementApi _apiClient;

        public WebSocketFrameHandler()
        {
            _apiClient = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig
            {
                ServiceURL = Environment.GetEnvironmentVariable("WEBSOCKETAPI_URL"),
            });
        }

        public Task HandleAsync(Frame frame, SessionContext context)
        {
            if (frame.FrameId % 30 == 0)
            {
                return SendLapData(frame, context);
            }
            if (frame.FrameId % 10 == 0 && frame.IsPlayer)
            {
                return SendTelemetryData(frame, context);
            }
            return Task.CompletedTask;
        }

        private async Task SendLapData(Frame frame, SessionContext context)
        {
            LambdaLogger.Log($"Sending lap frame {frame.FrameId} to WS Connections");
            var json = JsonSerializer.Serialize(WSLapData.CreateFromFrame(frame), new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var connections = await context.GetConnections();
            foreach (var connection in connections)
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    try
                    {
                        var response = await _apiClient.PostToConnectionAsync(new PostToConnectionRequest() { ConnectionId = connection, Data = stream });
                        LambdaLogger.Log($"Send message to WS Connection {connection}");
                    }
                    catch (AmazonServiceException e)
                    {
                        if (e.StatusCode == HttpStatusCode.Gone)
                        {
                            LambdaLogger.Log($"Connection {connection} is Gone");
                            await context.DeleteConnection(connection);
                        }
                        else
                        {
                            LambdaLogger.Log($"Failes sending frame {frame.FrameId}: {e.Message}");
                        }
                    }
                }
            }
        }

        private async Task SendTelemetryData(Frame frame, SessionContext context)
        {
            LambdaLogger.Log($"Sending telemetry frame {frame.FrameId} to WS Connections");
            var json = JsonSerializer.Serialize(WSTelemetryData.CreateFromFrame(frame), new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var connections = await context.GetConnections();
            foreach (var connection in connections)
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    try
                    {
                        var response = await _apiClient.PostToConnectionAsync(new PostToConnectionRequest() { ConnectionId = connection, Data = stream });
                        LambdaLogger.Log($"Send message to WS Connection {connection}");
                    }
                    catch (AmazonServiceException e)
                    {
                        if (e.StatusCode == HttpStatusCode.Gone)
                        {
                            LambdaLogger.Log($"Connection {connection} is Gone");
                            await context.DeleteConnection(connection);
                        }
                        else
                        {
                            LambdaLogger.Log($"Failes sending frame {frame.FrameId}: {e.Message}");
                        }
                    }
                }
            }
        }
    }
}
