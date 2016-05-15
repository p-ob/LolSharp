namespace LolSharp.RiotObjects.Status
{
    using System.Collections.Generic;

    public class Shard
    {
        public string Hostname { get; set; }
        public List<string> Locales { get; set; }
        public string Name { get; set; }
        public string RegionTag { get; set; }
        public string Slug { get; set; }
    }
}
