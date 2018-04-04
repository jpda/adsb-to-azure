using ADSB.Interpreter.Observers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace ADSB.Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", true)
                .Build();

            var deviceConnectionString = config["AzureIotHub:AccessKey"];
            var deviceId = config["AzureIotHub:DeviceId"];

            TelemetryConfiguration.Active.InstrumentationKey = config["ApplicationInsights:InstrumentationKey"];

            var ip = config["Device:IP"];
            var port = int.Parse(config["Device:Port"]);
            if (args.Any())
            {
                ip = args[0];
                if (args.Length > 1)
                {
                    port = int.Parse(args[1]);
                }
            }
            Console.WriteLine($"Connecting to {ip}:{port}...");
            Console.WriteLine($"Creating IoT Hub client for {deviceId}");

            var tc = new TelemetryClient();
            var logger = new AiLog(tc);
            var iotClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Amqp);
            var iotOb = new IotHubObserver(iotClient, logger);
            var consoleOb = new StatsObserver();
            var c = new Connector(ip, port, logger, iotOb, consoleOb);
        }
    }
}