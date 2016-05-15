using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotObjects.Status
{
    public class Shard
    {
        public string Hostname { get; set; }
        public List<string> Locales { get; set; }
        public string Name { get; set; }
        public string RegionTag { get; set; }
        public string Slug { get; set; }
    }
}
