using System.Threading.Tasks;
using Proto;

namespace backend.engine.Actors
{
    public class AppActor : IActor
    {
        private readonly Behavior _behavior;

        public AppActor()
        {
            _behavior = new Behavior(Unconfigured);
        }

        private Task Unconfigured(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                {
                    _behavior.Become(Configured);
                    break;
                }
            }

            return Actor.Done;
        }

        private Task Configured(IContext context)
        {
            switch (context.Message)
            {
                case Stopping _:
                {
                    _behavior.Become(ShuttingDown);
                    break;
                }
            }
            return Actor.Done;
        }

        private Task ShuttingDown(IContext context)
        {
            return Actor.Done;
        }

        public Task ReceiveAsync(IContext context)
        {
            return _behavior.ReceiveAsync(context);
        }
    }
}