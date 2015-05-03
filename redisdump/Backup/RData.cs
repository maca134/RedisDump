using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redisdump
{
    public abstract class RData
    {
        public int TTL { get; set; }
        public string Key { get; set; }
    }
}
