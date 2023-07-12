using DDNS.Api;
using DDNS.Api.Helper;
using DDNS.Api.Models;
using FastTunnel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDNS
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", true, true)
             .Build();

            var settings = conf.Get<Appsettings>();
            var dnsConfig = settings.DDNS;

            IDomainRecord domainRecord;
            switch (dnsConfig.openPlat)
            {
                case "aliyun":
                    domainRecord = new AliyunDomainRecord(dnsConfig);
                    break;
                default:
                    Console.WriteLine($"不支持的开放平台 {dnsConfig.openPlat}");
                    return;
            }

            while (true)
            {
                try
                {
                    Refresh(domainRecord, dnsConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(60 * 1000 * 10); // 1分钟检查一次
            }
        }

        public static void Refresh(IDomainRecord domainRecord, DDNSConfig dnsConfig)
        {
            var current_ip = IPHelper.CurrentIp();
            var records = domainRecord.GetRecords(dnsConfig.domain);
            if (string.IsNullOrEmpty(dnsConfig.RR.Trim()))
            {
                if (!string.IsNullOrEmpty(dnsConfig.ignoreRR.Trim()))
                {
                    var igs = dnsConfig.ignoreRR.Trim().Split("|");
                    records = records.Where(x => x.Type == "A" && !igs.Contains(x.RR));
                }
            }
            else
            {
                var rs = dnsConfig.RR.Trim().Split("|");
                records = records.Where(x => x.Type == "A" && rs.Contains(x.RR));
            }

            if (!records.Any(x => x.Value != current_ip))
            {
                Console.WriteLine($"{DateTime.Now} ip没有改变 {current_ip}");
                return;
            }

            Console.WriteLine($"{DateTime.Now} 更新A解析记录 {current_ip}");

            // 更新记录
            foreach (var item in records)
            {
                item.Value = current_ip;
                domainRecord.UpdateRecord(item);
            }
        }
    }
}
