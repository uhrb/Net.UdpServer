﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Net.UdpServer
{
    /// <summary>
    /// Simple server implementation, based on UdpClient
    /// </summary>
    public class Server : IServer
    {
        private readonly IOptions<ServerConfiguration> _config;
        private readonly ILogger _logger;
        private readonly IRequestContextFactory _factory;
        private long contextId = long.MinValue;

        /// <summary>
        /// Creates new instance of <see cref="Server"/>
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="logger">Logger</param>
        /// <param name="factory">Request factory</param>
        /// <exception cref="ArgumentNullException">When <paramref name="config"/>, <paramref name="factory"/> or <paramref name="logger"/> is null</exception>
        public Server(IOptions<ServerConfiguration> config, ILogger<Server> logger, IRequestContextFactory factory)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc/>
        public void Run(RequestDelegate pipeline, CancellationToken token)
        {
            if (_config.Value == null)
            {
                throw new InvalidOperationException("Configuration not provided");
            }
            _logger.LogInformation($"Starting udp server on {_config.Value.EndPoint}");
            //var sender = new IPEndPoint(IPAddress.Any, 0);
            using (var client = new UdpClient(_config.Value.EndPoint))
            {
                if (_config.Value.AllowNatTraversal != null)
                {
                    client.AllowNatTraversal(_config.Value.AllowNatTraversal.Value);
                }

                client.DontFragment = _config.Value.DontFragment;
                client.EnableBroadcast = _config.Value.EnableBroadcast;
                if (_config.Value.ExclusiveAddressUse != null)
                {
                    client.ExclusiveAddressUse = _config.Value.ExclusiveAddressUse.GetValueOrDefault();
                }
                client.MulticastLoopback = _config.Value.MulticastLoopback;
                client.Ttl = _config.Value.Ttl;
                while (token.IsCancellationRequested == false)
                {
                    var received = client.ReceiveAsync().GetAwaiter().GetResult();
                    _logger.LogDebug($"Received {received.Buffer.Length} bytes from {received.RemoteEndPoint}");
                    if (received.Buffer != null)
                    {
                        var id = Convert.ToBase64String(BitConverter.GetBytes(Interlocked.Increment(ref contextId)));
                        _logger.LogDebug($"{id} Context started");
                        var context = _factory.Create(received.RemoteEndPoint, received.Buffer, id);
                        Task.Run(async () =>
                        {
                            try
                            {
                                await pipeline(context);
                                if (context.ResponsePacket != null)
                                {
                                    _logger.LogDebug($"{id} Response generated by pipeline");
                                    var bytes = await client.SendAsync(context.ResponsePacket, context.ResponsePacket.Length, context.RemoteEndpoint);
                                    _logger.LogDebug($"{id} {bytes} bytes send to {context.RemoteEndpoint}");
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(exception: e, message: $"{id} Unhandled exception during pipeline execution");
                            }
                        });
                    }
                }
            }
        }
    }
}
