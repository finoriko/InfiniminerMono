using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
    class Program
    {
        private static void Main(string[] args)
        {
            var server = new Server();
            server.StartServer();
            server.ReadMessages();
        }
    }
}
