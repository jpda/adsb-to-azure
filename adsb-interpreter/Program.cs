using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ADSB.Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Connector("192.168.17.113", 30003);
        }
    }

    public class Connector
    {
        private readonly string _ip;
        private readonly int _port;
        private TcpClient _client;

        private long SessionId;

        List<Adsb.Message> _things = new List<Adsb.Message>();

        public Connector(string IP, int port)
        {
            _ip = IP;
            _port = port;
            Start(IP, port).Wait();
        }

        public async Task Start(string server, int port)
        {
            do
            {
                var connected = await Connect(server, port);
                if (connected)
                {
                    SessionId = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds * 100;
                    //Connected?.Invoke(this, new NewSessionEventArgs(SessionId.ToString()));
                    await ReadStream();
                }
                await Task.Delay(10000);
            } while (true);
        }

        private async Task<bool> Connect(string server, int port)
        {
            try
            {
                _client = new TcpClient();// server, port);
                await _client.ConnectAsync(server, port);
                //var prop = new Dictionary<string, string>() { { "Server", server }, { "Port", port.ToString() } };
                //Log.Event("TcpClient-Connected", prop);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                Console.WriteLine(ex);
                return false;
            }
        }

        private async Task ReadStream()
        {
            var data = new byte[256];
            var sbuffer = new StringBuilder(256);
            var mb = new MessageBuffer();
            var contCount = 0;

            try
            {
                while (_client.Connected)
                {
                    var stream = _client.GetStream();
                    if (!stream.DataAvailable)
                    {
                        //If data is not available in roughly 15 seconds.. disconnect
                        if (contCount > 15)
                        {
                            EndSession();
                            return;
                        }

                        await Task.Delay(1000);
                        contCount++;
                        continue;
                    }

                    contCount = 0;
                    var bytes = stream.Read(data, 0, data.Length);


                    sbuffer.Clear();
                    sbuffer.Append(Encoding.ASCII.GetString(data, 0, bytes));

                    //todo: parse this to a real object? maybe if we foresee lots of downstream subscribers in the future

                    mb.AddData(sbuffer.ToString()).ForEach(m =>
                    {
                        sbuffer.Clear().Append(m.Trim());
                        //todo: this is going to move to Async soon
                        //Parallel.ForEach(_observers, x =>
                        //{
                        //    x.OnNext($"{SessionId.ToString()};{sbuffer.ToString()}");
                        //});
                        var message = Adsb.Message.Parse(sbuffer.ToString());
                        _things.Add(message);
                        //Console.WriteLine($"Message Type: {message.GetType().Name}, TransmissionType: {message.TransmissionType}");
                    });

                    if (_things.Count > 10000)
                    {
                        EndSession();
                        var types = _things.GroupBy(x => x.GetType().Name).Select(y => new { Key = y.Key, Count = y.Count() }).OrderByDescending(z => z.Count);
                        foreach (var t in types)
                        {
                            Console.WriteLine($"{t.Key}: {t.Count}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Log.Error(e);
                //Log.Event("Stream read failed.");
                //Log.Event("TcpClient-Connection-Closed");
                Console.WriteLine(e);
                EndSession();
            }
        }

        private void EndSession()
        {
            _client.Dispose();
        }
    }

    public class MessageBuffer
    {
        private readonly List<string> _buffer = new List<string>();

        public List<string> AddData(string data)
        {
            if (_buffer.Any()) { data = $"{_buffer.First()}{data}"; }
            // <new> 
            // updated to parse record based on CRLF delimmiter
            //
            string[] delimiter = { "\r\n" };
            var lines = data.Split(delimiter, System.StringSplitOptions.None).Where(x => x.Length > 0).ToList();
            //var lines = data.Split("$".ToCharArray()).Where(x => x.Trim().Length > 0).Select(y => "$" + y).ToList();
            _buffer.Clear();
            var carryoverLine = lines.Last();
            _buffer.Add(carryoverLine);
            lines.Remove(carryoverLine);
            return lines;
        }
    }
}
