using System;
using System.Net;

namespace BranchSdk.Net
{
    public class BranchWebClient : WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            if (Timeout <= 100) Timeout = 2000;

            WebRequest webRequest = base.GetWebRequest(uri);
            webRequest.Timeout = Timeout;
            ((HttpWebRequest)webRequest).ReadWriteTimeout = Timeout;
            return webRequest;
        }
    }
}
