using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KScratch.Entity.Kudu
{
    public class KuduSiteSettings
    {
        public string SiteName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ApiUrl { get; set; }
        public string LiveUrl { get; set; }
    }
}
