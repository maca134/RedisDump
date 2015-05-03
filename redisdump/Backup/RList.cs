using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redisdump
{
    public class RList : RData
    {
        public List<string> Data { get; set; }

        internal void SaveTo(Redis redis)
        {
            foreach (string d in this.Data)
            {
                redis.RightPush(this.Key, Encoding.UTF8.GetBytes(d));
            }
            if (this.TTL > -1)
            {
                redis.Expire(this.Key, this.TTL);
            }
        }
    }
}
