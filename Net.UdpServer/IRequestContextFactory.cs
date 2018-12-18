using System.Net;

namespace Net.UdpServer
{
    /// <summary>
    /// Request context factory interface
    /// </summary>
    public interface IRequestContextFactory
    {
        /// <summary>
        /// Creates instance of request context
        /// </summary>
        /// <param name="remote">Remote endpoint, from which dgram was received</param>
        /// <param name="payload">Received payload</param>
        /// <param name="contextId">Assigned request context id</param>
        /// <returns><see cref="RequestContext"/></returns>
        RequestContext Create(IPEndPoint remote, byte[] payload, string contextId);
    }
}
