using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KScratch.Entity.Kudu;

namespace KScratch.Portable.Helpers
{
    public static class HttpHelpers
    {
        public static string GetAuthenticationString(KuduSiteSettings settings)
        {
            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{settings.UserName}:{settings.Password}"));
        }
    }
}
