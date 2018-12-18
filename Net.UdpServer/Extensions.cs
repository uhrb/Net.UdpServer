using Microsoft.Extensions.DependencyInjection;

namespace Net.UdpServer
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds default request context factory
        /// </summary>
        /// <param name="collection">Services</param>
        /// <returns>Services</returns>
        public static IServiceCollection AddDefaultRequestContextFactory(this IServiceCollection collection)
        {
            return collection.AddSingleton<IRequestContextFactory, DefaultRequestContextFactory>();
        }
    }
}
