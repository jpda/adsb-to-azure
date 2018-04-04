using System;
using System.Threading.Tasks;

namespace ADSB.Interpreter.Observers
{
    public interface IAsyncObserver<T> : IObserver<T>
    {
        Task OnNextAsync(T value);
    }
}