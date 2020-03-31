using InfiniminerMono;
using System;

namespace Infiniminer
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (InfiniminerGame game = new InfiniminerGame(args))
            {
                try
                {
                    game.Run();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message + "\r\n\r\n" + e.StackTrace);
                }
            }
        }
    }
#endif
}
