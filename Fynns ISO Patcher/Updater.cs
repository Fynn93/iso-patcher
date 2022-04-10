using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Net.Http;
using Downloader;
using System.Net;
using System.Reflection;
using ShellProgressBar;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Fynns_ISO_Patcher
{
    public static class Helper
    {
        public static string CalcMemoryMensurableUnit(this long bytes)
        {
            return CalcMemoryMensurableUnit((double)bytes);
        }

        public static string CalcMemoryMensurableUnit(this double bytes)
        {
            double kb = bytes / 1024; // · 1024 Bytes = 1 Kilobyte 
            double mb = kb / 1024; // · 1024 Kilobytes = 1 Megabyte 
            double gb = mb / 1024; // · 1024 Megabytes = 1 Gigabyte 
            double tb = gb / 1024; // · 1024 Gigabytes = 1 Terabyte 

            string result =
                tb > 1 ? $"{tb:0.##}TB" :
                gb > 1 ? $"{gb:0.##}GB" :
                mb > 1 ? $"{mb:0.##}MB" :
                kb > 1 ? $"{kb:0.##}KB" :
                $"{bytes:0.##}B";

            result = result.Replace("/", ".");
            return result;
        }
    }

    public class Updater
    {
        private static ProgressBar ConsoleProgress { get; set; }
        private static ProgressBarOptions ChildOption { get; set; }
        private static ProgressBarOptions ProcessBarOption { get; set; }
        private static ConcurrentDictionary<string, ChildProgressBar> ChildConsoleProgresses { get; set; }

        public static string GetVersion(string xml)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(xml);
            XmlNodeList xNodeList = xmlDoc.SelectNodes("/item");
            string s = xNodeList![0]!["version"]!.InnerText;
            return s;
        }

        public static string GetURL(string xml)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(xml);
            XmlNodeList xNodeList = xmlDoc.SelectNodes("/item");
            string s = xNodeList![0]!["url"]!.InnerText;
            return s;
        }

        public static async Task Update()
        {
            HttpClient client = new();
            var task = Task.Run(() => client.GetStringAsync("https://api.fynn93.tech/naas/update.xml"));
            task.Wait();
            string xml = task.Result.ToString();
            string version = GetVersion(xml);
            string url = GetURL(xml);
            string oversion = File.ReadAllText(".\\bin\\version.txt");
            int oversioni = Convert.ToInt32(oversion.Replace(".", ""));
            int versioni = Convert.ToInt32(version.Replace(".", ""));
            if(oversioni < versioni)
            {
                var downloadOpt = new DownloadConfiguration()
                {
                    MaxTryAgainOnFailover = int.MaxValue,
                    OnTheFlyDownload = false,
                    ParallelDownload = true,
                    TempDirectory = "C:\\temp",
                    Timeout = 1000,
                    RequestConfiguration =
                    {
                        Accept = "*/*",
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                        CookieContainer =  new CookieContainer(), // Add your cookies
                        Headers = new WebHeaderCollection(), // Add your custom headers
                        KeepAlive = false,
                        ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
                        UseDefaultCredentials = false,
                        UserAgent = $"DownloaderSample/{Assembly.GetExecutingAssembly().GetName().Version!.ToString(3)}"
                    }
                };
                var downloader = new DownloadService(downloadOpt);
                downloader.DownloadStarted += OnDownloadStarted;
                downloader.DownloadProgressChanged += OnDownloadProgressChanged;
                downloader.DownloadFileCompleted += OnDownloadFileCompleted;
                
                Directory.CreateDirectory("update");
                await downloader.DownloadFileTaskAsync(url, "update\\download.zip");
            }
        }

        private static void OnDownloadStarted(object sender, DownloadStartedEventArgs e)
        {
            ConsoleProgress = new ProgressBar(10000,
                $"Downloading {Path.GetFileName(e.FileName)} ...", ProcessBarOption);
            ChildConsoleProgresses = new ConcurrentDictionary<string, ChildProgressBar>();
        }

        private static void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ConsoleProgress?.Tick(10000);
            Console.WriteLine();
            Console.WriteLine();

            if (e.Cancelled)
            {
                Console.WriteLine("Download canceled!");
            }
            else if (e.Error != null)
            {
                Console.Error.WriteLine(e.Error);
            }
            else
            {
                Console.WriteLine("Download completed successfully.\n\n");
                Commands.System(".\\bin\\tools\\post-update.bat", "");
                Environment.Exit(0);
            }
        }

        private static void OnDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            ConsoleProgress.Tick((int)(e.ProgressPercentage * 100));
        }
    }
}
