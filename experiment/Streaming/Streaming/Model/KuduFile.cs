using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Model
{
    public class KuduFile
    {
        public string name { get; set; }
        public int size { get; set; }
        public string mtime { get; set; }
        public string crtime { get; set; }
        public string mime { get; set; }
        public string href { get; set; }
        public string path { get; set; }
    }
}
