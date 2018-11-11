using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace VisualBoxManager
{
    static class HttpClientFactory
    {
        private static CookieContainer cookieContainer = new CookieContainer();
        public static HttpClient getCLient(Auth auth)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            handler.CookieContainer = cookieContainer;
        
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip");
            client.DefaultRequestHeaders.Add("accept", "application/json");
            
            if (auth.authenticated())
            {
                cookieContainer.Add(Config.getUri(), new Cookie("JSESSIONID", auth.getSessionId()));
               // client.DefaultRequestHeaders.Add("cookie", auth.getSessionIdForCookie());
            }

            return client;
        }

        public static HttpClient getCLient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip");
            client.DefaultRequestHeaders.Add("accept", "application/json");

            return client;
        }
    }
}
