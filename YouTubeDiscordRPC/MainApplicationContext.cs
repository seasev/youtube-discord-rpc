using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeDiscordRPC
{
    class MainApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public MainApplicationContext()
        {
            this.trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };
        }

        void Exit(object sender, EventArgs e)
        {
            this.trayIcon.Visible = false;

            Application.Exit();
        }
    }
}
