using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redisdump
{
    public class RString : RData
    {
        public string Data { get; set; }

        internal void SaveTo(Redis redis)
        {
            redis.Set(this.Key, this.Data);
            if (this.TTL > -1)
            {
                redis.Expire(this.Key, this.TTL);
            }
        }
    }
}
