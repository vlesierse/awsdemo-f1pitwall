namespace F1Pitwall.WebSocketApi.Models
{
    public class Message<T>
    {
        public string Action { get; set; }

        public T Payload { get; set; }
     }
}
