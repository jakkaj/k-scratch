using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ks.model.Services
{
    public class ParamService : IParamService
    {
        Dictionary<string, string> _params = new Dictionary<string, string>();
        
        public void Add(string name, string value)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value)){
                return;
            }

            _params.Add(name, value);
        }

        public string Get(string name)
        {
            if (!_params.ContainsKey(name))
            {
                return null;
            }

            return _params.FirstOrDefault(_=>_.Key == name).Value;
        }
    }
}
