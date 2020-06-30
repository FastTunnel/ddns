using System;
using System.Collections.Generic;
using System.Text;

namespace DDNS.Api.Models
{
    public class DDNSConfig
    {
        public string openPlat { get; set; }

        public string accessKey { get; set; }

        public string accessSecret { get; set; }

        public string domain { get; set; }

        public string ignoreRR { get; set; }
    }
}
