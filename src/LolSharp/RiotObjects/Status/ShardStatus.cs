using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotObjects.Status
{
    public class ShardStatus : Shard
    {
        public List<Service> Services { get; set; }
    }
}
