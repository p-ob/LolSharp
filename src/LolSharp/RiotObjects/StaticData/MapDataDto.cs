using System.Collections.Generic;

namespace LolSharp.RiotObjects.StaticData
{
    public class MapDataDto
    {
        public Dictionary<string, MapDetailsDto> Data { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }
}
