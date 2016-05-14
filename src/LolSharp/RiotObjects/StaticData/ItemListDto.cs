using System.Collections.Generic;

namespace LolSharp.RiotObjects.StaticData
{
    public class ItemListDto
    {
        public BasicDataDto Basic { get; set; }

        public Dictionary<string, ItemDto> Data { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<ItemTreeDto> Tree { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }
}
