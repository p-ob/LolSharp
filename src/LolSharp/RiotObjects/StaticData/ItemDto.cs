using System.Collections.Generic;

namespace LolSharp.RiotObjects.StaticData
{
    public class ItemDto : BasicDataDto
    {
        public Dictionary<string, string> Effect { get; set; }
    }
}
