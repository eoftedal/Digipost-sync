using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Digipostsync.Core
{
    public class SyncFile
    {
        public FileDescriptor FileDescriptor { get; private set; }
        public FileInfo Path { get; private set; }

        public SyncFile(FileInfo path, FileDescriptor fileDescriptor)
        {
            Path = path;
            FileDescriptor = fileDescriptor;
        }
    }
}

