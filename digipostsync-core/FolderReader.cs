using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Digipostsync.Core
{
    public class FolderReader
    {
        private String _path;
        public FolderReader(String path)
        {
            _path = path;
        }
        public IEnumerable<SyncFile> Read()
        {
            var folder = new DirectoryInfo(_path);
            var files = folder.GetFiles("*.pdf");
            return files.Select(f => new SyncFile(f, FileDescriptor.FromFile(f.FullName)));
        }
    }
}
