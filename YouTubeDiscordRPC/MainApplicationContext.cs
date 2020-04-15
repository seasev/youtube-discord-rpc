using System;
using System.Windows.Forms;

namespace YouTubeDiscordRPC
{
    class MainApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        private MenuItem stateItem;
        private MenuItem exitItem;

        public MainApplicationContext()
        {
            stateItem = new MenuItem("Stop", Stop);
            exitItem = new MenuItem("Exit", Exit);

            this.trayIcon = new NotifyIcon()
            {
                Text = "YouTube Discord RPC Client",
                Icon = Properties.Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Stop", Stop),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            this.trayIcon.Click += NotifyIcon_Click;
            this.trayIcon.DoubleClick += Exit;
        }

        // Change the context menu items based on RPC state
        void UpdateContextMenu()
        {
            trayIcon.ContextMenu.Dispose();
            trayIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                DiscordClient.IsRunning ? new MenuItem("Stop", Stop) : new MenuItem("Start", Start),
                new MenuItem("Exit", Exit)
            });
        }

        // Pause the RPC
        void Stop(object sender, EventArgs e)
        {
            DiscordClient.Stop();
            UpdateContextMenu();
        }

        // Resume the RPC
        void Start(object sender, EventArgs e)
        {
            DiscordClient.Start();
            UpdateContextMenu();
        }

        // Exit the application
        void Exit(object sender, EventArgs e)
        {
            DiscordClient.Exit();

            this.trayIcon.Visible = false;
            Application.Exit();
        }

        // Runs when the tray icon is clicked
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var eventArgs = e as MouseEventArgs;

            switch (eventArgs.Button)
            {
                case MouseButtons.Left:
                    DiscordClient.Toggle();
                    UpdateContextMenu();
                    break;
            }
        }
    }
}
