using System;
using System.Threading.Tasks;

namespace ADSB.Interpreter.Observers
{
    public abstract class ObserverBase<T> : IAsyncObserver<T>
    {
        ILog _log;
        public ObserverBase(ILog logger)
        {
            _log = logger;
        }
        public virtual void OnCompleted()
        {
            _log.Info($"Observer completed");
        }

        public virtual void OnError(Exception error)
        {
            _log.Error(error);
        }

        public virtual void OnNext(T value)
        {
            //_log.Event("MessageReceived");
        }

        public virtual Task OnNextAsync(T value)
        {
            OnNext(value);
            return Task.CompletedTask;
        }
    }
}