using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDNS.Api.Helper
{
    public class IPHelper
    {
        public static string CurrentIp()
        {
            // http://ifconfig.me/ip
            // http://ipinfo.io/ip
            // https://ifconfig.co/ip
            var res = HttpHelper.GetAsync("http://ipinfo.io/ip").Result;

            var mc = Regex.Match(res, @"\d+.\d+.\d+.\d+");
            if (mc.Success)
            {
                return mc.Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
