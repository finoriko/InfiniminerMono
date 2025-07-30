using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniminerServer
{
    class Program
    {
        static void RunServer()
        {
            bool restartServer = true;
            while (restartServer)
            {
                InfiniminerServer infiniminerServer = new InfiniminerServer();
                restartServer = infiniminerServer.Start();
            }
        }
        static void Main(string[] args)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                RunServer();
            }
            else
            {
                try
                {
                    RunServer();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Server Error: " + e.Message);
                    Console.WriteLine("Stack Trace: " + e.StackTrace);
                    if (!Console.IsInputRedirected)
                    {
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
