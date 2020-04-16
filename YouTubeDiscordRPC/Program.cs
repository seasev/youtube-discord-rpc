using System;
using System.Windows.Forms;

namespace YouTubeDiscordRPC
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DiscordClient.Initialize();

            YoutubeHandler.Initialize();
            YoutubeHandler.Start();
            
            Application.Run(new MainApplicationContext());
        }
    }
}
