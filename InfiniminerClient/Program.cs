using System;

namespace InfiniminerMono
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (InfiniminerGame game = new InfiniminerGame(args))
            {
                try
                {
                    game.Run();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Client Error: " + e.Message);
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
