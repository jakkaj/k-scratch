using System;
using System.Collections.Generic;
using System.Text;

namespace ks.model.Entity.TestSystem
{
    public class FunctionTest
    {
        /// <summary>
        /// The response code that will be sent back by the admin URL
        /// </summary>
        public int ExpectedResponseCode { get; set; }

        /// <summary>
        /// The response text that will be sent back from the admin URL.  
        /// </summary>
        public string ExpectedResposeText { get; set; }

        /// <summary>
        /// The key that will be passed in the x-functions-key header
        /// </summary>
        public string AdminKey { get; set; }

        /// <summary>
        /// Extra headers that will be send along to the input of the 
        /// </summary>
        public Dictionary<string,string> Headers { get; set; }
        public string Input { get; set; }
        public string PostBody { get; set; }
    }
}
