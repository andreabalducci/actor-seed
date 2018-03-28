using System;
using System.Threading.Tasks;
using backend.engine.Actors;
using Proto;

namespace backend.engine
{
    public class Engine : IEngine
    {
        private PID _root;
        private readonly IActorFactory _actorFactory;

        public Engine(IActorFactory actorFactory)
        {
            _actorFactory = actorFactory;
        }

        public void Start()
        {
            this._root = _actorFactory.GetActor<AppActor>("app");
        }

        public void Stop()
        {
            this._root?.Stop();
        }

        public void Post(object message)
        {
            this._root.Tell(message);
        }

        public Task<T> QueryAsync<T>(object query)
        {
            return this._root.RequestAsync<T>(query);
        }
    }

    public interface IEngine
    {
        void Start();
        void Stop();
        void Post(object message);
        Task<T> QueryAsync<T>(object query);
    }
}
