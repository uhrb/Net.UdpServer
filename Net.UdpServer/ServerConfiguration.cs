using System.Net;

namespace Net.UdpServer
{
    public class ServerConfiguration
    {
        /// <summary>
        /// Sets or gets server endpoint, to bind to
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

        /// <summary>
        /// Gets or sets value, if NAT traversal enabled. Default - unspecified
        /// </summary>
        public bool? AllowNatTraversal { get; set; }

        /// <summary>
        /// Sets or gets value, is fragmentation allowed. Default is true.
        /// </summary>
        public bool DontFragment { get; set; }

        /// <summary>
        /// Gets or sets value, indicating broadcast messaging enabled. Default is false
        /// </summary>
        public bool EnableBroadcast { get; set; }

        /// <summary>
        /// Gets or sets value, indicating address usage is exclusive. Default is true.
        /// </summary>
        public bool ExclusiveAddressUse { get; set; }

        /// <summary>
        /// Gets or sets value, indicating multicast messages returned to application itself. Default is false.
        /// </summary>
        public bool MulticastLoopback { get; set; }

        /// <summary>
        /// Gets or sets TTL. Default is 255.
        /// </summary>
        public short Ttl { get; set; }

        /// <summary>
        /// Creates new instance of <see cref="ServerConfiguration"/> with default values
        /// </summary>
        public ServerConfiguration()
        {
            DontFragment = true;
            EnableBroadcast = false;
            ExclusiveAddressUse = true;
            MulticastLoopback = false;
            Ttl = 255;
        }
    }
}
