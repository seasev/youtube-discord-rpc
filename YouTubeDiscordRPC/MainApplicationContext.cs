using System;
using System.Windows.Forms;

namespace YouTubeDiscordRPC
{
    class MainApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        private const string infoMsg = "This is a Discord Rich Presence client for Discord made by SagaFreya. It gets information about any YouTube videos you're watching and shows them under the \"Playing\" section of your Discord profile.";
        private const string infoCap = "YouTube Rich Presence Client - Information";

        public MainApplicationContext()
        {

            this.trayIcon = new NotifyIcon()
            {
                Text = "YouTube Discord RPC Client",
                Icon = Properties.Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Info", Info),
                    new MenuItem("Stop", Stop),
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            this.trayIcon.Click += NotifyIcon_Click;
            this.trayIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        // Change the context menu items based on RPC state
        void UpdateContextMenu()
        {
            trayIcon.ContextMenu.Dispose();
            trayIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Info", Info),
                DiscordClient.IsRunning ? new MenuItem("Stop", Stop) : new MenuItem("Start", Start),
                new MenuItem("Exit", Exit)
            });
        }

        // Pause the RPC
        void Stop(object sender, EventArgs e)
        {
            YoutubeHandler.Stop();
            DiscordClient.Stop();
            UpdateContextMenu();
        }

        // Resume the RPC
        void Start(object sender, EventArgs e)
        {
            DiscordClient.Start();
            YoutubeHandler.Start();
            UpdateContextMenu();
        }

        // Run start or stop based on application state
        void Toggle(object sender, EventArgs e)
        {
            if (DiscordClient.IsRunning)
                Stop(sender, e);
            else
                Start(sender, e);
        }

        // Exit the application
        void Exit(object sender, EventArgs e)
        {
            YoutubeHandler.Exit();
            DiscordClient.Exit();

            this.trayIcon.Visible = false;
            Application.Exit();
        }

        void Info(object sender, EventArgs e)
        {
            MessageBox.Show(infoMsg, infoCap, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Runs when the tray icon is clicked
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var eventArgs = e as MouseEventArgs;

            switch (eventArgs.Button)
            {
                case MouseButtons.Left:
                    Toggle(sender, e);
                    break;
            }
        }

        // Runs when the tray icon is double-clicked
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            var eventArgs = e as MouseEventArgs;

            switch (eventArgs.Button)
            {
                case MouseButtons.Left:
                    Exit(sender, e);
                    break;
            }
        }
    }
}
