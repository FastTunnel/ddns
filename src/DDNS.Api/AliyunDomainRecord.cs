using Aliyun.Acs.Alidns.Model.V20150109;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using DDNS.Api.Helper;
using DDNS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDNS.Api
{
    public class AliyunDomainRecord : IDomainRecord
    {
        private string accessKey;
        private string accessSecret;

        public AliyunDomainRecord(DDNSConfig config)
        {
            this.accessKey = config.accessKey;
            this.accessSecret = config.accessSecret;
        }

        public IEnumerable<DescribeRecord> GetRecords(string domainName)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKey, accessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);

            var request = new DescribeDomainRecordsRequest();
            request.DomainName = domainName;
            request.TypeKeyWord = "A";

            var response = client.GetAcsResponse(request);
            if (response.TotalCount == 0)
            {
                throw new Exception("请先手动解析几条A记录");
            }

            return response.DomainRecords.Select(x => new DescribeRecord()
            {
                RecordId = x.RecordId,
                Value = x.Value,
                RR = x.RR,
                Type = x.Type,
            });
        }

        public void UpdateRecord(DescribeRecord record)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKey, accessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);

            var request = new UpdateDomainRecordRequest();
            request.RecordId = record.RecordId;
            request.RR = record.RR;
            request.Type = record.Type;
            request.Value = record.Value;
            try
            {
                var response = client.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
