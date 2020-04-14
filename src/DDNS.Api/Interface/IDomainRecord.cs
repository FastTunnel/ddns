using DDNS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDNS.Api
{
    public interface IDomainRecord
    {
        IEnumerable<DescribeRecord> GetRecords(string domainName);

        void UpdateRecord(DescribeRecord record);
    }
}
