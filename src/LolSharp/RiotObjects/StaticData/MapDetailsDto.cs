using System.Collections.Generic;

namespace LolSharp.RiotObjects.StaticData
{
    public class MapDetailsDto
    {
        public ImageDto Image { get; set; }
        public long MapId { get; set; }
        public string MapName { get; set; }
        public List<long> UnpurchasableItemList { get; set; }
    }
}
