using System.Threading;
using System.Threading.Tasks;

namespace Net.UdpServer
{
    /// <summary>
    /// Udp Server interface
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Runs server using supplied pipeline for processing requests
        /// </summary>
        /// <param name="pipeline">Pipeline</param>
        /// <param name="token">Cancellation token</param>
        Task Run(RequestDelegate pipeline, CancellationToken token);
    }
}
