using System;
using System.IO;
using System.Threading.Tasks;

namespace Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            const string downloadFolder = @"d:\Data\Downloads";
            var dt = new DownloadTask(downloadFolder, new Uri("https://www.edsm.net/dump/systemsWithCoordinates.json"));
            var dtTask = Task.Run<FileInfo>(() => dt.DownloadAsync());
            dtTask.Wait();
            var fi = dtTask.Result;
        }
    }
}
