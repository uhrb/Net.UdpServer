using System.Net;

namespace Net.UdpServer
{
    /// <summary>
    /// Default request context factory
    /// </summary>
    public class DefaultRequestContextFactory : IRequestContextFactory
    {
        /// <inheritdoc/>
        public RequestContext Create(IPEndPoint remote, byte[] payload, string contextId)
        {
            return new RequestContext(remote, payload, contextId);
        }
    }
}
