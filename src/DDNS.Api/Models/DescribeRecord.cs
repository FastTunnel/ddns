using System;
using System.Collections.Generic;
using System.Text;

namespace DDNS.Api.Models
{
    public class DescribeRecord
    {
        public string RecordId { get; set; }
        public string Value { get; set; }
        public string RR { get; set; }
        public string Type { get; set; }
    }
}
