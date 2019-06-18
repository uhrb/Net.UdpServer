using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net.UdpServer.Echo
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(config =>
            {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Trace);
            });
            // for example using Microsoft.Extensions.Logging.Console;
            services.AddOptions<ServerConfiguration>().Configure(config => {
                config.EndPoint = new IPEndPoint(IPAddress.Any, 1337);
            });
            services.AddDefaultRequestContextFactory();
            services.AddSingleton<IServer, Server>();
            var provider = services.BuildServiceProvider();
            var pipeline = new PipelineBuilder(provider)
                .Use((context, next) => {
                    context.ResponsePacket = context.RequestPacket;
                    Console.WriteLine(Encoding.ASCII.GetString(context.RequestPacket));
                    return Task.CompletedTask;
                })
                .Build();
            var server = provider.GetRequiredService<IServer>();
            var source = new CancellationTokenSource();
            var tsk = Task.Run(() => server.Run(pipeline, source.Token));
            Console.WriteLine("Sending packet...");
            using(var client = new UdpClient(6666, AddressFamily.InterNetwork))
            {
                var bytes = Encoding.ASCII.GetBytes("Hello!");
                client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("127.0.01"), 1337));
            }
            Console.WriteLine("Packet sended");
            Console.ReadLine();
            source.Cancel();
            Task.WhenAll(tsk).Wait();
        }
    }
}
