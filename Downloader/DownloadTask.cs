using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Downloader
{
    public class DownloadTask
    {
        public String Folder { get; private set; }
        public Uri Uri { get; private set; }

        public DownloadTask(string folder, Uri uri)
        {
            Folder = folder;
            Uri = uri;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public async Task<FileInfo> DownloadAsync()
        {
            var wr = HttpWebRequest.Create(Uri);
            wr.Method = "HEAD";
            using (var response = await wr.GetResponseAsync())
            {
                var downloadFile = false;
                var fi = new FileInfo(Path.Combine(Folder, new FileInfo(Uri.LocalPath).Name));
                if (!fi.Exists)
                {
                    downloadFile = true;
                }
                else if (response.ContentLength != fi.Length)
                {
                    downloadFile = true;
                }

                if (downloadFile)
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(Uri, fi.FullName);
                    }
                }

                return fi;
            }
        }
    }
}
