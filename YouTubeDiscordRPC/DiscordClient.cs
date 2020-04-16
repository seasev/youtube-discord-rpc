using DiscordRPC;
using DiscordRPC.Logging;
using System;

namespace YouTubeDiscordRPC
{
    static class DiscordClient
    {
        // Important variables and constants
        private const string clientID = "699671462117638166";

        private const LogLevel logLevel = LogLevel.Info;

        private static int discordPipe = -1;

        private static readonly RichPresence idlePresence = new RichPresence()
        {
            State = "Idle",
            Assets = new Assets
            {
                LargeImageKey = "squircle_red",
                LargeImageText = "Idle"
            }
        };

        private static DiscordRpcClient client;

        public static bool IsRunning { get; private set; } = false;


        // Main program
        public static void Initialize(int pipe = -1)
        {
            discordPipe = pipe;

            Connect();
        }


        // Presence example
        private static void Connect()
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

            ClearVideo();

            IsRunning = true;
        }

        // Set a new presence based on playing video
        public static void SetVideo(YoutubeVideo video)
        {
            if (video.Idle)
            {
                idlePresence.Timestamps = Timestamps.Now;
                client.SetPresence(idlePresence);
            }
            else
                client.SetPresence(new RichPresence
                {
                    Details = video.Title,
                    State = "Watching",
                    Timestamps = Timestamps.Now,
                    Assets = new Assets
                    {
                        LargeImageKey = "squircle_red",
                        LargeImageText = "Watching a Video"
                    }
                });
        }


        // If YouTube isn't open, clear the presence
        public static void ClearVideo()
        {
            client.ClearPresence();
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
            IsRunning = true;
        }


        // Destroy the client
        public static void Exit()
        {
            client.Dispose();
        }
    }
}
