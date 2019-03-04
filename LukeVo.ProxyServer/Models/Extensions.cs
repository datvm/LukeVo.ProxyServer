using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{

    internal static class Extensions
    {

        public static bool IsSameOrigin(this Uri uri, Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var port = request.Host.Port.GetPortOrDefault(uri.Scheme);

            return uri.Scheme == request.Scheme && uri.Host == request.Host.Host && uri.Port == port;
        }

        public static int? GetPortOrDefault(this int? port, string scheme)
        {
            if (port == null)
            {
                switch (scheme)
                {
                    case "http":
                        port = 80;
                        break;
                    case "https":
                        port = 443;
                        break;
                }
            }

            return port;
        }

    }

}