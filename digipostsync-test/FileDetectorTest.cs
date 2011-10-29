using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Digipostsync.Core;
using System.IO;

namespace Digipostsync.Test
{
    [TestFixture]
    public class FileDetectorTest
    {
        private Random _rnd = new Random();
        String _path1 = "file1.pdf";
        String _path2 = "file2.pdf";

        [SetUp]
        public void Init()
        {
            File.Delete(_path1);
            File.Delete(_path2);
        }

        [Test]
        public void ShouldDetectEqualFiles()
        {
            CreateRandomFile(_path1);

            File.Copy(_path1, _path2);

            FileDescriptor fileDescriptor1 = FileDescriptor.FromFile(_path1);
            FileDescriptor fileDescriptor2 = FileDescriptor.FromFile(_path2);

            Assert.AreEqual(fileDescriptor1, fileDescriptor2);

        }
        [Test]
        public void ShouldDetectInEqualFiles()
        {
            CreateRandomFile(_path1);
            CreateRandomFile(_path2);

            FileDescriptor fileDescriptor1 = FileDescriptor.FromFile(_path1);
            FileDescriptor fileDescriptor2 = FileDescriptor.FromFile(_path2);

            Assert.AreNotEqual(fileDescriptor1, fileDescriptor2);
        }
        [Test]
        public void ShouldDetectInequalFilesWhenSameHashButDifferentSize()
        {
            var hash = "a";

            FileDescriptor fileDescriptor1 = new FileDescriptor(hash, 1000, "abc.pdc");
            FileDescriptor fileDescriptor2 = new FileDescriptor(hash, 1001, "abc.pdc");

            Assert.AreNotEqual(fileDescriptor1, fileDescriptor2);
        }

        private void CreateRandomFile(String path)
        {
            List<byte> fileBytes = new List<byte>();
            int length = _rnd.Next(100, 1000);
            for (int i = 0; i < length; i++)
            {
                fileBytes.Add((byte)_rnd.Next(0, 256));
            }
            File.WriteAllBytes(path, fileBytes.ToArray());
        }
    }
}
