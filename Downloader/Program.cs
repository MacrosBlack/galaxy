using System;
using System.IO;
using System.Threading.Tasks;

namespace Downloader
{
    class Program
    {
        static int Main(string[] args)
        {
            const string downloadFolder = @"d:\Data\Downloads";
            var dt = new DownloadTask(downloadFolder, new Uri("https://www.edsm.net/dump/systemsWithCoordinates.json"));
            var task = Task.Run<FileInfo>(async () => await dt.DownloadAsync());
            task.Wait();
            var fi = task.Result;

            dt = new DownloadTask(downloadFolder, new Uri("https://www.edsm.net/dump/stations.json"));
            task = Task.Run<FileInfo>(async () => await dt.DownloadAsync());
            task.Wait();
            fi = task.Result;

            return 0;
        }
    }
}
