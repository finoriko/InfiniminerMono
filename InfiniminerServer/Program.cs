using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniminerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool restartServer = true;
            InfiniminerServer infiniminerServer = new InfiniminerServer();
            restartServer = infiniminerServer.StartServer();
            //server.ReadMessages();
        }
    }
}
