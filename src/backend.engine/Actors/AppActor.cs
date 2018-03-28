using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using backend.engine.Messages;
using Proto;
using Proto.Schedulers.SimpleScheduler;

namespace backend.engine.Actors
{
    public class AppActor : IActor
    {
        private readonly Behavior _behavior;
        private readonly ISimpleScheduler _scheduler;
        private CancellationTokenSource _cts;
        private readonly IDictionary<string, PID> _clients = new Dictionary<string, PID>();
        private readonly IActorFactory _actorFactory;

        public AppActor(ISimpleScheduler scheduler, IActorFactory actorFactory)
        {
            _scheduler = scheduler;
            _actorFactory = actorFactory;
            _behavior = new Behavior(Unconfigured);
        }

        private Task Unconfigured(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                {
                    _scheduler.ScheduleTellRepeatedly(
                        delay: TimeSpan.FromSeconds(10),
                        interval: TimeSpan.FromSeconds(10),
                        target: context.Self,
                        message: Tick.Instance,
                        cancellationTokenSource: out _cts
                    );
                    _behavior.Become(Configured);
                    break;
                }
            }

            return Actor.Done;
        }

        private async Task Configured(IContext context)
        {
            switch (context.Message)
            {
                case Tick tick:
                {
                    foreach (var pid in context.Children)
                    {
                        context.Tell(pid, tick);
                    }

                    break;
                }

                case ReceiveData data:
                {
                    GetClient(data.ClientId, context).Tell(data);
                    break;
                }

                case QueryData query:
                {
                    context.Respond(await GetClient(query.ClientId, context).RequestAsync<ReceiveData>(query).ConfigureAwait(false));
                    break;
                }

                case Stopping _:
                {
                    _cts.Cancel();
                    _behavior.Become(ShuttingDown);
                    break;
                }
            }
        }

        private PID GetClient(string clientId, IContext context)
        {
            if (!_clients.TryGetValue(clientId, out PID pid))
            {
                pid = _actorFactory.GetActor<ClientDataActor>(clientId, null, context);
                _clients[clientId] = pid;
            }

            return pid;
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