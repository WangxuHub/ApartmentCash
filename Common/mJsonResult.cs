using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class mJsonResult
    {
        public bool success = false;
        public string msg = "";
        public object rows;
        public object obj;
        public int total = 0;

        public override string  ToString()
        {
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return str;
        }
    }

    
}
