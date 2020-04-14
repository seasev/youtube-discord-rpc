using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YouTubeDiscordRPC
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            DateTime startTime = DateTime.Now;
            
            DiscordClient.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new MainApplicationContext());

            Console.WriteLine("All Threads Exited in {0} secods", (DateTime.Now - startTime).TotalSeconds);
            Console.ReadKey();
        }
    }
}
