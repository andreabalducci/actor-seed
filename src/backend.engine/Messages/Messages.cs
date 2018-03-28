using System;

namespace backend.engine.Messages
{
    public sealed class Tick
    {
        public static readonly Tick Instance = new Tick();
        private Tick()
        {
        }
    }

    public class ReceiveData
    {
        public string ClientId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Data { get; set; }
    }

    public class QueryData
    {
        public string ClientId { get; set; }
    }

}