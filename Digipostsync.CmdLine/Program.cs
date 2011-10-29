using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digipostsync.Core;

namespace Digipostsync.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DigipostSync s = new DigipostSync();
                s.FileDownloaded += (uri, filename) => Console.WriteLine("Downloaded " + uri + " to " + filename);
                s.FileUploaded += (filename) => Console.WriteLine("Uploading " + filename);
                s.Syncronize(args[0], args[1], args[2]);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR:" + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
