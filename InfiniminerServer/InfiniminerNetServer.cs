using Lidgren.Network;

namespace InfiniminerServer
{
    public class InfiniminerNetServer : NetServer
    {
        public InfiniminerNetServer(NetConfiguration config)
           : base(config)
        {
        }

        /* crappy hack to fix duplicate key error crash in Lidgren, hopefully a new
         * version of Lidgren will fix this issue. */
        public bool SanityCheck(NetConnection connection)
        {
            if (this.m_connections.Contains(connection) == false)
            {
                if (this.m_connectionLookup.ContainsKey(connection.RemoteEndPoint))
                {
                    this.m_connectionLookup.Remove(connection.RemoteEndPoint);
                    return true;
                }
            }

            return false;
        }
    }
}