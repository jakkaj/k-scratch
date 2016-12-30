using System;
using System.Text;
using ks.model.Entity.Kudu;

namespace ks.model.Helpers
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
