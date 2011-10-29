using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Digipostsync.Core
{
    public static class DigipostLogin
    {
        public static CookieContainer Login(String username, String password)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.digipost.no/");
            request.CookieContainer = request.CookieContainer ?? new CookieContainer();
            using(var r = request.GetResponse()) {
            }
            var cookies = request.CookieContainer;

            request = (HttpWebRequest)WebRequest.Create("https://www.digipost.no/post/passordautentisering");
            request.CookieContainer = cookies;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            String data = "rolle=PRIVAT&foedselsnummer=" + Uri.EscapeDataString(username) + "&passord=" + Uri.EscapeDataString(password);
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }
            try
            {
                using (var r = request.GetResponse())
                {
                }
            }
            catch (WebException wex)
            {

            }
            return cookies;
        }
    }
}
