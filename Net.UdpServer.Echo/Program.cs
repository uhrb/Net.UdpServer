using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Net.UdpServer.Echo
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(); // for example using Microsoft.Extensions.Logging.Console;
            services.AddOptions<ServerConfiguration>().Configure(config => {
                config.EndPoint = new IPEndPoint(IPAddress.Any, 1337);
            });
            services.AddDefaultRequestContextFactory();
            services.AddSingleton<IServer, Server>();
            var provider = services.BuildServiceProvider();
            var pipeline = new PipelineBuilder(provider)
                .Use((context, next) => {
                    context.ResponsePacket = context.RequestPacket;
                    return Task.CompletedTask;
                })
                .Build();
            var server = provider.GetRequiredService<IServer>();
            server.Run(pipeline, CancellationToken.None);
        }
    }
}
