using Lidgren.Network;

namespace InfiniminerServer
{
    public class InfiniminerNetServer : NetServer
    {
        public InfiniminerNetServer(NetPeerConfiguration config)
           : base(config)
        {
        }

        /* crappy hack to fix duplicate key error crash in Lidgren, hopefully a new
         * version of Lidgren will fix this issue. */
        public bool SanityCheck(NetConnection connection)
        {
            if (this.Connections.Contains(connection) == false)
            {
                if (this.Connections.Contains(connection))
                {
                    this.Connections.Remove(connection);
                    return true;
                }
            }

            return false;
        }
    }
}