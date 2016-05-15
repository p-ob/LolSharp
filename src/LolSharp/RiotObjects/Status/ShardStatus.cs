namespace LolSharp.RiotObjects.Status
{
    using System.Collections.Generic;

    public class ShardStatus : Shard
    {
        public List<Service> Services { get; set; }
    }
}
