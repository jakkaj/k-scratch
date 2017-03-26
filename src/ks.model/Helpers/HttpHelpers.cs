using System;
using System.Text;
using ks.model.Entity.Kudu;
using System.Collections.Generic;

namespace ks.model.Helpers
{
    public static class HttpHelpers
    {
        public static Dictionary<string,string> GetAuthHeadersForGoo(KuduSiteSettings settings)
        {
            var authString = GetAuthenticationString(settings);

            return new Dictionary<string, string>
            {
                {
                   "Authorization", "Basic " + authString
                }
            };
        }
        public static string GetAuthenticationString(KuduSiteSettings settings)
        {
            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{settings.UserName}:{settings.Password}"));
        }
    }
}
