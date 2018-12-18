using System.Collections.Concurrent;
using System.Net;

namespace Net.UdpServer
{
    /// <summary>
    /// Simple request context
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Gets or sets protocol identifier
        /// </summary>
        public long? ProtocolId { get; set; }

        /// <summary>
        /// Gets context id
        /// </summary>
        public string ContextId { get; }

        /// <summary>
        /// Gets request packet
        /// </summary>
        public byte[] RequestPacket { get; }

        /// <summary>
        /// Gets or sets response packet contents. If value is set, server may send it to the RemoteEndpoint
        /// </summary>
        public byte[] ResponsePacket { get; set; }

        /// <summary>
        /// Remote endpoint, from which dgram was receieved
        /// </summary>
        public IPEndPoint RemoteEndpoint { get; }

        /// <summary>
        /// Dictionary with custom values, may be set by middlewares
        /// </summary>
        public ConcurrentDictionary<object, object> Values { get; }

        /// <summary>
        /// Creates instance of <see cref="RequestContext"/>
        /// </summary>
        /// <param name="remote">Remote endpoint</param>
        /// <param name="request">Request bytes</param>
        /// <param name="contextId">Context id</param>
        public RequestContext(IPEndPoint remote, byte[] request, string contextId)
        {
            Values = new ConcurrentDictionary<object, object>();
            RequestPacket = request;
            RemoteEndpoint = remote;
            ContextId = contextId;
        }
    }
}