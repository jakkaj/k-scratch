using System;
using System.Collections.Generic;
using System.Text;

namespace ks.model.Entity.Kudu.TestData
{
    public class Key
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class FunctionKey
    {
        public List<Key> keys { get; set; }
        public List<Link> links { get; set; }
    }
}
