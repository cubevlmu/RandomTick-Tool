using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Name3.ApplicationCommon
{
    class ApplicationInit : IDisposable
    {
        private AssetsDownloader Downloader { get; } = AssetsDownloader.CreateDownloader();
        private AppConfigration Config { get; } = AppConfigration.Get();
        public ApplicationInit()
        {
            if (!Directory.Exists("Lang"))
                Directory.CreateDirectory("Lang"); 
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            GC.Collect();
        }

        public bool Init()
        {
            try
            {
                string url = Config.SyncUrl;
                string response = Downloader.DownloadString(url);
                JObject root = JObject.Parse(response);
                string Version = (string)root["Version"];
                string Date = (string)root["Date"];

                if (AppInfo.Date != Date || AppInfo.Version != Version)
                {
                    if (Config.AutoUpdate)
                    {
                        MessageBox.Show("New Version!Click The Sure Button To Get Update \b What's new? Please See The Project Github Page");
                        Process.Start((string)root["Url"]);
                    }
                }
                DownloadHelpPages(root);
                DownloadLanguages(root);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        private async void DownloadHelpPages(JObject root)
        {
            try
            {
                var help = root["HelpPage"].ToString();
                help = Config.SyncUrl.Replace("Index.json", help);
                Downloader.PostTasks(new AssetsTask()
                {
                    Url = help,
                    Place = $"Data\\HelpPageFile",
                    OnDownloadDone = new Action(() => { GC.Collect(); }),
                    OnDownloadFaild = new Action(() => {
                        MessageBox.Show($"Error When Downloading The Assets \n Please Check Your Network Connection \n Or Conntact To FlyBird Studio \n FileName HelpPageFile \n ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    })
                });
                await Downloader.StartActionAsync(); 
                AppInfo.HelpPageUrl = new Uri($"file:///{Application.StartupPath}/Data/HelpPageFile", UriKind.RelativeOrAbsolute);
            }
            catch (Exception) { }
        }

        private async void DownloadLanguages(JObject root)
        {
            var Language = (JArray)root["Language"];
            foreach (var Lang in Language)
            {
                var name = (string)Lang["name"];
                var url = (string)Lang["url"];
                url = Config.SyncUrl.Replace("Index.json", url);

                if (File.Exists($"Lang\\{name}.lang"))
                    continue;

                Downloader.PostTasks(new AssetsTask()
                {
                    Url = url,
                    Place = $"Lang\\{name}.lang",
                    OnDownloadFaild = new Action(() =>
                    {
                        MessageBox.Show($"Error When Downloading The Assets \n Please Check Your Network Connection \n Or Conntact To FlyBird Studio \n FileName {name} \n ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }),
                    OnDownloadDone = new Action(() =>
                    {
                        GC.Collect();
                    })
                });
            }
            await Downloader.StartActionAsync();
            GC.Collect();
        }

        public void Dispose()
        {
            Downloader.Dispose();
            GC.Collect();
            GC.SuppressFinalize(Downloader);
            GC.WaitForFullGCComplete();
        }

        ~ApplicationInit()
        {
            Dispose();
        }
    }
}
