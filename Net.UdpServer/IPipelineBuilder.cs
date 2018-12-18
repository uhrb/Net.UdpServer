using System;
using System.Threading.Tasks;

namespace Net.UdpServer
{
    /// <summary>
    /// Server pipeline builder
    /// </summary>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Service provider
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Adds middleware into pipeline
        /// </summary>
        /// <param name="middleware">The middleware</param>
        /// <returns><see cref="IPipelineBuilder"/></returns>
        IPipelineBuilder Use(Func<RequestContext, Func<Task>, Task> middleware);

        /// <summary>
        /// Adds middleware into pipeline
        /// </summary>
        /// <param name="middleware">The middleware</param>
        /// <returns><see cref="IPipelineBuilder"/></returns>
        IPipelineBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);

        /// <summary>
        /// Builds pipeline from midlwares
        /// </summary>
        /// <returns></returns>
        RequestDelegate Build();
    }
}
