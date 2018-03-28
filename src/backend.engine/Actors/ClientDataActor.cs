using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using backend.engine.Messages;
using Proto;

namespace backend.engine.Actors
{
    public class ClientDataActor : IActor
    {
        private ReceiveData _lastReceivedData;

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case ReceiveData data:
                {
                    _lastReceivedData = data;
                    break;
                }

                case QueryData query:
                { 
                    context.Respond(_lastReceivedData);
                    break;
                }

                case Tick tick:
                {
                    // check timeout?
                    break;
                }
            }

            return Actor.Done;
        }
    }
}
