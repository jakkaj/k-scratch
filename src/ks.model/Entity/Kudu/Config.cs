using System.Collections.Generic;

namespace ks.model.Entity.Kudu
{
    public class Config
    {
        public bool disabled { get; set; }
        public string scriptFile { get; set; }
        public string entryPoint { get; set; }
        public List<Binding> bindings { get; set; }
    }
}
