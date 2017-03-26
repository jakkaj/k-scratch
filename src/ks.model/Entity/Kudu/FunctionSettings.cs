using System;
using System.Text;

namespace ks.model.Entity.Kudu
{

    public class FunctionSettings
    {
        public string name { get; set; }
        public object function_app_id { get; set; }
        public string script_root_path_href { get; set; }
        public string script_href { get; set; }
        public string config_href { get; set; }
        public string secrets_file_href { get; set; }
        public string href { get; set; }
        public Config config { get; set; }
        public object files { get; set; }
        public string test_data { get; set; }
    }
}
