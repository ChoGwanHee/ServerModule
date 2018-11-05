using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System;

namespace ServerModule
{
    class PhpManager
    {
        private static string _url;
        public static string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public static void GetHttp(string _http)
        {
            // string http = "http://192.168.0.10:13000/login.php?type=insert%Nick=" + Nick;
            URL = _http;
        }

        public void InsertNickname(string _nick, int _id)
        {
            InstanceValue.Nickname = _nick;
            InstanceValue.ID = _id;
        }

        public void Web()
        {
            WebRequest _request = WebRequest.Create(URL);
            WebResponse _response = _request.GetResponse();
        }

        private string Http(WebRequest _request, WebResponse _response)
        {
            Stream _data = _response.GetResponseStream();
            StreamReader _reader = new StreamReader(_data);

            return _reader.ReadToEnd().Trim();
        }
    }
}
