using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace Digipostsync.Core
{
    public class FileDescriptor
    {
        private static SHA1 sha1 = SHA1.Create();
        public String Hash { get; private set; }
        public long Length { get; private set; }
        public String Filename { get; private set; }

        public FileDescriptor(byte[] fileBytes, String filename)
            :this(Convert.ToBase64String(sha1.ComputeHash(fileBytes)), fileBytes.Length, filename)
        {
        }
        public FileDescriptor(String hash, long length, String filename)
        {
            Hash = hash;
            Length = length;
            Filename = filename;
        }

        public static FileDescriptor FromFile(string path)
        {
            var fileBytes = File.ReadAllBytes(path);
            return new FileDescriptor(fileBytes, new FileInfo(path).Name);
        }
        public static FileDescriptor FromUri(Uri uri, DigipostClient client)
        {
            String filename;
            var data = client.DownloadFile(uri, out filename);
            return new FileDescriptor(data, filename);
        }
        public static bool operator ==(FileDescriptor desc1, FileDescriptor desc2)
        {
            if ((object)desc1 == null)
            {
                return (object)desc2 == null;
            }
            return desc1.Equals(desc2);
        }
        public static bool operator !=(FileDescriptor desc1, FileDescriptor desc2)
        {
            if ((object)desc1 == null)
            {
                return (object)desc2 != null;
            }
            return !desc1.Equals(desc2);
        }
        public override bool Equals(object obj)
        {
            var desc = obj as FileDescriptor;
            if (desc != null)
            {
                return desc.Hash == Hash && desc.Length == Length;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Hash + Length).GetHashCode();
        }
    }
}
