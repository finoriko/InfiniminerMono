using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.StartClient();

            while (true)
            {
                var text = Console.ReadLine();
                client.SendMessage(text);

                if (text == "exit")
                {
                    break;
                }
            }

            client.Disconnect();
        }
    }
}
