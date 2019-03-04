using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukeVo.ProxyServer.Models
{

    public class ProxyServerSettings
    {

        public IEnumerable<ProxySettings> Proxy { get; set; }

        public class ProxySettings
        {
            public IEnumerable<string> From { get; set; }
            public string To { get; set; }

            internal IEnumerable<Uri> FromUri { get; set; }
        }

    }

}
