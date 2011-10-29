using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Digipostsync.Core
{
    public class DigipostClient
    {
        private CookieContainer _sessionCookies;
        public String Token { get; set; }
        public DigipostClient(CookieContainer sessionCookies)
        {
            _sessionCookies = sessionCookies;
        }

        public byte[] DownloadFile(Uri uri, out String filename)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = _sessionCookies;
            using (var response = request.GetResponse())
            {
                filename = response.Headers["Content-Disposition"]
                    .Split(';').Where(s => s.Trim().StartsWith("filename="))
                    .First()
                    .Split(new[] { '=' }, 2)[1];
                using (var memoryStream = new MemoryStream())
                {
                    response.GetResponseStream().CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public String DownloadString(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = _sessionCookies;
            using (var response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        internal void UploadFile(Uri uri, SyncFile f)
        {
            NameValueCollection nvc = new NameValueCollection();
            String emne = f.Path.Name;
            emne = new Regex(@"[^a-zA-Z0-9.\- ]").Replace(emne, "");
            nvc.Add("emne", emne);
            nvc.Add("token", Token);

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.CookieContainer = _sessionCookies;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "fil", f.Path.Name, "application/octet-stream");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(f.Path.FullName, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();


            using (var wresp = wr.GetResponse())
            {

            }
        }
    }

}