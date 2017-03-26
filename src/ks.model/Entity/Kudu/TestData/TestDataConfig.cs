using System;
using System.Collections.Generic;
using System.Text;

namespace ks.model.Entity.Kudu.TestData
{

    public class NameValuePair
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class TestDataConfig
    {
        public List<object> availableMethods { get; set; }
        public List<NameValuePair> queryStringParams { get; set; }
        public List<NameValuePair> headers { get; set; }
        public string method { get; set; }
        public string body { get; set; }
    }
}
