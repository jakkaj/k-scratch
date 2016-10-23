using System;
using System.Text;
using ks.Model.Entity.Kudu;

namespace ks.Model.Helpers
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
