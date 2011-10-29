using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Digipostsync.Core
{
    public class DigipostSync
    {
        public delegate void FileUploadEventHandler(string fullPath);
        public delegate void FileDownloadEventHandler(string uri, String newName);

        public event FileUploadEventHandler FileUploaded;
        public event FileDownloadEventHandler FileDownloaded;


        public void Syncronize(String username, String password, String path)
        {
            var cookies = DigipostLogin.Login(username, password);
            var cache = path + @"\" + "cache.json";
            if (!(new FileInfo(cache).Exists)) {
                File.WriteAllText(cache, "[]");
            }
            var dpClient = new DigipostClient(cookies);
            var remoteFiles = new DigipostReader(dpClient, cache).Read();
            var localFiles = new FolderReader(path).Read();
            var filesToDownload = remoteFiles.Where(f => !localFiles.Any(l => l.FileDescriptor == f.FileDescriptor));
            var filesToUpload = localFiles.Where(f => !remoteFiles.Any(l => l.FileDescriptor == f.FileDescriptor));

            foreach(var f in filesToDownload) {
                String filename;
                var data = dpClient.DownloadFile(new Uri(f.Uri), out filename);
                File.WriteAllBytes(path + @"\" + filename.Replace("\"", ""), data);
                if (FileDownloaded != null)
                {
                    FileDownloaded.Invoke(f.Uri, filename);
                }
            }
            foreach (var f in filesToUpload)
            {
                Uri uri = new Uri("https://www.digipost.no/post/privat/dokument");
                dpClient.UploadFile(uri, f);
                if (FileUploaded != null)
                {
                    FileUploaded.Invoke(f.Path.FullName);
                }
            }
        }
    }

 
}
