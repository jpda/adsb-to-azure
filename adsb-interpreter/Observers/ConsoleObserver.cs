using adsb = ADSB.Interpreter.Adsb;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ADSB.Interpreter.Observers
{
    public class ConsoleObserver : IAsyncObserver<adsb.Message>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Console observer completed");
        }

        public void OnError(Exception error)
        {
            var def = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error.Message);
            Console.ForegroundColor = def;
        }

        public void OnNext(adsb.Message value)
        {
            var val = JsonConvert.SerializeObject(value);
            Console.WriteLine(val);
        }

        public Task OnNextAsync(adsb.Message value)
        {
            OnNext(value);
            return Task.CompletedTask;
        }
    }
}