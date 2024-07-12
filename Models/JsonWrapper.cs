using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class JsonWrapper
    {
        public JsonWrapper() { }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int GMC { get; set; }
        public List<AddressWrapper> address { get; set; }

    }

    public class AddressWrapper
    {
        public AddressWrapper() { }
        public string? line1 { get; set; }
        public string? city { get; set; }
        public string? postcode { get; set; }
    }
}
