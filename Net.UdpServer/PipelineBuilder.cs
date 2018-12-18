using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net.UdpServer
{
    /// <summary>
    /// Simple pipeline builder realization
    /// </summary>
    public class PipelineBuilder : IPipelineBuilder
    {
        /// <summary>
        /// List of middleware
        /// </summary>
        private readonly IList<Func<RequestDelegate, RequestDelegate>> _components;

        /// <summary>
        /// Creates new instance of <see cref="PipelineBuilder"/>
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        /// <exception cref="ArgumentNullException">When <paramref name="serviceProvider"/> is null</exception>
        public PipelineBuilder(IServiceProvider serviceProvider)
        {
            _components = new List<Func<RequestDelegate, RequestDelegate>>();
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc/>
        public IPipelineBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder Use(Func<RequestContext, Func<Task>, Task> middleware)
        {
            return Use(next =>
            {
                return context =>
                {
                    Task nxt() => next(context);
                    return middleware(context, nxt);
                };
            });
        }

        /// <inheritdoc/>
        public RequestDelegate Build()
        {
            RequestDelegate app = context => Task.CompletedTask;
            foreach (var component in _components.Reverse())
            {
                app = component(app);
            }

            return app;
        }
    }
}
