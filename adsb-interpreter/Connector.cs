using ADSB.Interpreter.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ADSB.Interpreter
{
    public class Connector
    {
        private readonly string _ip;
        private readonly int _port;
        private TcpClient _client;
        private List<IAsyncObserver<Adsb.Message>> _observers;
        private ILog _log;

        public Connector(string IP, int port, ILog log, params IAsyncObserver<Adsb.Message>[] observers)
        {
            _ip = IP;
            _port = port;
            _log = log;
            _observers = observers.ToList();
            Start(IP, port).Wait();
        }

        public async Task Start(string server, int port)
        {
            do
            {
                var connected = await Connect(server, port);
                if (connected)
                {
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
                var prop = new Dictionary<string, string>() { { "Server", server }, { "Port", port.ToString() } };
                _log.Event("TcpClient-Connected", prop);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
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

                    mb.AddData(sbuffer.ToString()).ForEach(m =>
                    {
                        sbuffer.Clear().Append(m.Trim());
                        Parallel.ForEach(_observers, async x =>
                        {
                            await x.OnNextAsync(Adsb.Message.Parse(sbuffer.ToString()));
                        });
                    });
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
                _log.Event("Stream read failed.");
                _log.Event("TcpClient-Connection-Closed");
                Console.WriteLine(e);
                EndSession();
            }
        }

        private void EndSession()
        {
            _client.Dispose();
        }
    }
}