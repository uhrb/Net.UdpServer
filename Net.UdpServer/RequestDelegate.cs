using System.Threading.Tasks;

namespace Net.UdpServer
{
    /// <summary>
    /// Request pipeline delegate
    /// </summary>
    /// <param name="context">The context of request</param>
    /// <returns><see cref="Task"/></returns>
    public delegate Task RequestDelegate(RequestContext context);
}