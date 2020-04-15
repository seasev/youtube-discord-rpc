using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Text;

namespace YouTubeDiscordRPC
{
    static class DiscordClient
    {
        // Important variables and constants
        private const string clientID = "699671462117638166";

        private static LogLevel logLevel = LogLevel.Info;

        private static int discordPipe = -1;

        private static readonly RichPresence idlePresence = new RichPresence()
        {
            State = "Idle"
        };

        private static DiscordRpcClient client;

        public static bool IsRunning { get; private set; } = false;

        private static StringBuilder word = new StringBuilder();


        // Main program
        public static void Initialize(int pipe = -1)
        {
            discordPipe = pipe;

            MainPresence();
        }


        // Presence example
        private static void MainPresence()
        {
            client = new DiscordRpcClient(clientID, pipe: discordPipe)
            {
                Logger = new DiscordRPC.Logging.ConsoleLogger(logLevel, true)
            };

            client.OnReady += (sender, msg) =>
            {
                Console.WriteLine("Connected to discord with user {0}", msg.User.Username);
            };

            client.OnPresenceUpdate += (sender, msg) =>
            {
                Console.WriteLine("Presence updated");
            };

            client.Initialize();

            UpdatePresence();

            IsRunning = true;
        }

        // Update the presence in Discord
        private static void UpdatePresence()
        {
            client.SetPresence(idlePresence);
        }

        // Stop showing the rich presence in Discord
        public static void Stop()
        {
            client.ClearPresence();

            IsRunning = false;
        }

        // Re-enable the rich presence
        public static void Start()
        {
            UpdatePresence();

            IsRunning = true;
        }

        // Toggle the state of the rich presence
        public static void Toggle()
        {
            if (IsRunning)
                Stop();
            else
                Start();
        }

        // Destroy the client
        public static void Exit()
        {
            client.Dispose();
        }
    }
}
