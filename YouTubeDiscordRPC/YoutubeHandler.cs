using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace YouTubeDiscordRPC
{
    static class YoutubeHandler
    {
        private const int interval = 10000;

        private static readonly string[] browsers = { "opera", "chrome", "iexplore", "firefox" };

        private static Timer timer;

        private static YoutubeVideo currentVideo = null;


        // Create the timer
        public static void Initialize()
        {
            timer = new Timer
            {
                Interval = interval
            };

            timer.Tick += Timer_Tick;
        }


        // Functions to start, stop, and exit the timer
        public static void Start()
        {
            timer.Start();
        }

        public static void Stop()
        {
            timer.Stop();
            currentVideo = null;
        }

        public static void Exit()
        {
            timer.Stop();
            timer.Dispose();
        }
        

        // Runs every time the timer has elapsed to its interval
        private static void Timer_Tick(object sender, EventArgs e)
        {
            ScanProcesses();
        }


        // Scan the processes for anything YouTube-related
        private static void ScanProcesses()
        {
            Console.WriteLine("Scanning for YouTube...");

            Process[] processList = Process.GetProcesses();

            Process finalProcess = null;

            // Check all the window titles for "YouTube"
            foreach (Process process in processList)
            {
                if (process.MainWindowTitle.ToLower().Contains("youtube") && IsBrowser(process))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                    // Stop at the first match - no more than one YouTube window is scanned
                    finalProcess = process;
                    break;
                }
            }

            // Make sure YouTube is open
            if (finalProcess != null)
            {
                YoutubeVideo video = ParseVideo(finalProcess.MainWindowTitle);
                // Check if video details have changed
                if (currentVideo == null || !(currentVideo.Title == video.Title && currentVideo.Idle == video.Idle))
                {
                    // Only if the video is different, update the presence
                    currentVideo = video;
                    DiscordClient.SetVideo(video);
                }
            }
            else
            {
                // Clear presence if nothing is playing
                currentVideo = null;
                DiscordClient.ClearVideo();
            }
        }


        // Function to check if a process is a known web browser
        private static bool IsBrowser(Process process)
        {
            if (browsers.Contains(process.ProcessName.ToLower()))
                return true;

            return false;
        }


        // Get details about the video from the window title
        private static YoutubeVideo ParseVideo(string windowTitle)
        {
            YoutubeVideo video = new YoutubeVideo();

            string[] terms = windowTitle.Split(new string[] { " - " }, StringSplitOptions.None);

            // If there is no video title, assume user is on the YouTube homepage
            if (terms.Length <= 2)
            {
                video.Idle = true;
                return video;
            }

            // Get the video title
            Array.Resize(ref terms, terms.Length - 2);
            string title = string.Join(" - ", terms);
            video.Title = title;

            return video;
        }
    }
}
