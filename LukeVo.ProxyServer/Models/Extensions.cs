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
            return uri.Scheme == request.Scheme && uri.Host == request.Host.Host && uri.Port == request.Host.Port;
        }

    }

}
