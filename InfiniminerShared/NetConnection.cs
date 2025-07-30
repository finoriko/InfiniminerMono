using System.Net;

namespace InfiniminerShared
{
    public abstract class NetConnection
    {
        public abstract IPEndPoint RemoteEndPoint { get; }
        public abstract void Disconnect();
        public abstract object Status { get; }
    }
}