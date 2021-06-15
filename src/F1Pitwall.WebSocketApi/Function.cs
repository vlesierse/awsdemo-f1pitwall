using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using F1Pitwall.WebSocketApi.Models;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace F1Pitwall.WebSocketApi
{
    public class Function
    {
        private IAmazonDynamoDB _dynamodbClient;

        public Function()
        {
            _dynamodbClient = new AmazonDynamoDBClient();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log(JsonSerializer.Serialize(request));
            try
            {
                if (request.RequestContext.EventType == "CONNECT")
                {
                    await OnConnect(request, context);
                }
                if (request.RequestContext.EventType == "DISCONNECT")
                {
                    await OnDisconnect(request, context);
                }
                else if (request.RequestContext.EventType == "MESSAGE")
                {
                    await OnMessage(request, context);
                }
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = "Ok"
                };
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Error connecting: " + e.Message);
                LambdaLogger.Log(e.StackTrace);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Failed: {e.Message}"
                };
            }
        }

        private Task OnConnect(APIGatewayProxyRequest request, ILambdaContext context)
        {
            /*var connectionId = request.RequestContext.ConnectionId;
            LambdaLogger.Log("Connected using id:" + connectionId);
            Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>() {
                    { "PK", new AttributeValue{ S = $"CX:{connectionId}" } },
                    { "SK", new AttributeValue{ S = $"CX:{connectionId}" } }
                };
            var putItemRequest = new PutItemRequest()
            {
                TableName = "f1pitwall-sessions",
                Item = attributes
            };
            return _dynamodbClient.PutItemAsync(putItemRequest);*/
            return Task.CompletedTask;
        }

        private Task OnDisconnect(APIGatewayProxyRequest request, ILambdaContext context)
        {
            /*var connectionId = request.RequestContext.ConnectionId;
            LambdaLogger.Log("Disconnected:" + connectionId);
            Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>() {
                    { "PK", new AttributeValue{ S = $"CX:{connectionId}" } },
                    { "SK", new AttributeValue{ S = $"CX:{connectionId}" } }
                };
            var deleteItem = new DeleteItemRequest()
            {
                TableName = "f1pitwall-sessions",
                Key = attributes
            };
            return _dynamodbClient.DeleteItemAsync(deleteItem);*/
            return Task.CompletedTask;
        }

        private Task OnMessage(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var header = JsonSerializer.Deserialize<MessageHeader>(request.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            switch(header.Action)
            {
                case "JOIN":
                    return HandleMessage(JsonSerializer.Deserialize<Message<Join>>(request.Body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), request);
                default:
                    LambdaLogger.Log("Unknown message: " + header.Action);
                    return Task.CompletedTask;
            }
        }

        private Task HandleMessage(Message<Join> message, APIGatewayProxyRequest request)
        {
            Dictionary<string, AttributeValue> attributes = new Dictionary<string, AttributeValue>() {
                    { "PK", new AttributeValue{ S = $"S#{message.Payload.Session}" } },
                    { "SK", new AttributeValue{ S = $"CX#{request.RequestContext.ConnectionId}" } }
                };
            var putItemRequest = new PutItemRequest()
            {
                TableName = "f1pitwall-sessions",
                Item = attributes
            };
            return _dynamodbClient.PutItemAsync(putItemRequest);
        }

    }
}
