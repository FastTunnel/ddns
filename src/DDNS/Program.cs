using DDNS.Api;
using DDNS.Api.Helper;
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
                    Refresh(domainRecord, dnsConfig.domain);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(60 * 1000 * 10); // 1分钟检查一次
            }
        }

        public static void Refresh(IDomainRecord domainRecord, string domain)
        {
            var current_ip = IPHelper.CurrentIp();
            var records = domainRecord.GetRecords(domain);
            if (current_ip == records.First().Value)
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
