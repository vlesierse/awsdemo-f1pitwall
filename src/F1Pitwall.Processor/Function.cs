using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Amazon.Lambda.Serialization.SystemTextJson;
using F1Pitwall.Models;
using F1Pitwall.Processor.Handlers;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace F1Pitwall.Processor
{
    public class Function
    {
        private readonly FrameProcessor _frameProcessor;

        public Function()
        {
            _frameProcessor = new FrameProcessor(new WebSocketFrameHandler());
        }

        public async Task FunctionHandlerAsync(KinesisEvent kinesisEvent, ILambdaContext context)
        {
            context.Logger.LogLine($"Beginning to process {kinesisEvent.Records.Count} records...");
            _frameProcessor.Start();
            var tasks = kinesisEvent.Records.Select(async record =>
            {
                context.Logger.LogLine($"Event ID: {record.EventId}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                string json = await GetRecordContents(record.Kinesis);
                var frame = JsonSerializer.Deserialize<Frame>(json, new JsonSerializerOptions
                {
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return _frameProcessor.Process(frame);
            });
            await Task.WhenAll(tasks);
            context.Logger.LogLine("Stream processing complete.");
        }

        private Task<string> GetRecordContents(KinesisEvent.Record streamRecord)
        {
            using (var reader = new StreamReader(streamRecord.Data, Encoding.ASCII))
            {
                return reader.ReadToEndAsync();
            }
        }
    }
}