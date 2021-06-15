using Amazon.DynamoDBv2;
using F1Pitwall.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F1Pitwall.Processor
{
    public class FrameProcessor
    {
        private readonly IEnumerable<IFrameHandler> _handlers;
        private readonly ConcurrentDictionary<string, SessionContext> _sessions;
        private readonly AmazonDynamoDBClient _dynamodbClient;

        public FrameProcessor(IEnumerable<IFrameHandler> handlers)
        {
            _handlers = handlers ?? Enumerable.Empty<IFrameHandler>();
            _dynamodbClient = new AmazonDynamoDBClient();
            _sessions = new ConcurrentDictionary<string, SessionContext>();
        }

        public FrameProcessor(params IFrameHandler[] handlers) : this(handlers.AsEnumerable()) { }

        public void Start()
        {
            _sessions.Clear();
        }
        
        public Task Process(Frame frame)
        {

            var context =_sessions.GetOrAdd(frame.SessionId.ToString(), (session) => new SessionContext(_dynamodbClient, session));
            return Task.WhenAll(_handlers.Select(h => h.HandleAsync(frame, context)));
        }
    }
}
