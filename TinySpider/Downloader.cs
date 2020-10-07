using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using TinySpider.Base;

namespace TinySpider
{
    public class Downloader : IDownloader
    {
        private IWebProxyPool ProxyPool = null;
        public Downloader(IWebProxyPool proxyPool = null)
        {
            ProxyPool = proxyPool;
            System.Net.ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(RemoteCertificateValidationCallback);
        }

        public Encoding Encoding { get; set; } = Encoding.Default;

        public string RequestUrl(Uri url)
        {
            Debug.WriteLine("IDownloader -> " + url);

            using (var http = new WebClient())
            {
                //http.Timeout = this.Timeout;

                http.Proxy = ProxyPool?.GetProxy();
                http.Encoding = this.Encoding;

                http.Headers.Add(HttpRequestHeader.Accept, "text/html");
                //装一下User-Agent?
                http.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)");
                var html = http.DownloadString(url);

                return html;
            }
        }


        //允许https
        bool RemoteCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                              System.Security.Cryptography.X509Certificates.X509Chain chain,
                              System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }



    }
}
