using DDNS.Api;
using DDNS.Api.Helper;
using System;
using System.Linq;
using System.Threading;

namespace DDNS
{
    class Program
    {
        static void Main(string[] args)
        {
            string api = "aliyun";
            string accessKey = "LTAI4FttR8xEbARtfW1CZRk2";
            string accessSecret = "a3drBv41y9fCwdZAsGjPaMb9mNsVU4";
            string domain = "ioxygen.cc";

            IDomainRecord domainRecord;
            switch (api)
            {
                case "aliyun":
                    domainRecord = new AliyunDomainRecord(accessKey, accessSecret);
                    break;
                default:
                    Console.WriteLine($"不支持的开放平台 {api}");
                    return;
            }

            while (true)
            {
                try
                {
                    Refresh(domainRecord, domain);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(60 * 1000); // 1分钟检查一次
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
