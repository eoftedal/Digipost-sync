using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Digipostsync.Core
{
    public class DigipostReader
    {
        private String _settingsFile;
        private DigipostClient _digipostClient;

        public DigipostReader(DigipostClient digipostClient, String settingsFile)
        {
            _digipostClient = digipostClient;
            _settingsFile = settingsFile;
        }
        public IEnumerable<DigipostLocalFile> Read()
        {
            var localFilesJson = File.ReadAllText(_settingsFile);
            var localFiles = JSONHelper.Deserialise<DigipostLocalFile[]>(localFilesJson);
            var remoteFilesJson = DownloadListOfFiles();
            var remoteJsonFiles = JSONHelper.Deserialise<DigipostFile[]>(remoteFilesJson);
            var remoteFiles = remoteJsonFiles.Where(f => f.Selvopplastet).Select(f => Resolve(f, localFiles));
            localFilesJson = JSONHelper.Serialise(remoteFiles.ToArray());
            File.WriteAllText(_settingsFile, localFilesJson);
            return remoteFiles;
        }

        private DigipostLocalFile Resolve(DigipostFile remoteFile, DigipostLocalFile[] localFiles)
        {
            var cachedResult = localFiles.FirstOrDefault(f => f.Uri == remoteFile.BrevUri);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            var result = new DigipostLocalFile();
            result.Uri = remoteFile.BrevUri + "?download";
            var descriptor = FileDescriptor.FromUri(new Uri(result.Uri), _digipostClient);
            result.Hash = descriptor.Hash;
            result.Length = descriptor.Length;
            result.Filename = descriptor.Filename;
            return result;
        }

        private String DownloadListOfFiles()
        {

            var kontoJson = _digipostClient.DownloadString(new Uri("https://www.digipost.no/post/privat/konto"));
            var konto = JSONHelper.Deserialise<DigipostKonto>(kontoJson);
            var arkivUri = konto.ArkivUri;
            _digipostClient.Token = konto.Token;
            return _digipostClient.DownloadString(new Uri(arkivUri));
        }

    }
}
