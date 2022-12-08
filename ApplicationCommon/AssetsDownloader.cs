using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Name3.ApplicationCommon
{
    class AssetsDownloader : IDisposable
    {
        public static AssetsDownloader CreateDownloader()
        {
            return new AssetsDownloader();
        }

        private WebClient Client { get; } = new WebClient();
        private List<AssetsTask> Tasks { get; } = new List<AssetsTask>();
        public int NowIndex { get; private set; } = 0;

        private AssetsDownloader()
        { }

        public void PostTasks(AssetsTask task)
            => Tasks.Add(task);

        public string DownloadString(string url)
        {
            byte[] datas = Client.DownloadData(url);
            return Encoding.UTF8.GetString(datas);
        }

        public Task StartActionAsync()
        {
            int count = Tasks.Count - 1;
            NowIndex = 0;
            foreach (AssetsTask task in Tasks)
            {
                try
                {
                    Client.DownloadFile(task.Url, task.Place);
                    task.OnDownloadDone();
                } catch (Exception e)
                {
                    task.OnDownloadFaild();
                    Console.WriteLine(e.ToString());
                    throw new Exception($"Catch An Exception \n{e}");
                }
                NowIndex++;
                GC.Collect();
            }
            Tasks.RemoveRange(0, count);
            GC.Collect();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Tasks.Clear();
            Client.Dispose();
            GC.SuppressFinalize(Client);
            GC.Collect();
            GC.WaitForFullGCApproach();
        }

        ~AssetsDownloader()
        {
            Dispose();
        }
    }
}
