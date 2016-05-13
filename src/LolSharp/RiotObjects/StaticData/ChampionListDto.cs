namespace LolSharp.RiotObjects.StaticData
{
    using System.Collections.Generic;

    public class ChampionListDto
    {
        public Dictionary<string, ChampionDto> Data { get; set; }
        public string Format { get; set; }
        public Dictionary<string, string> Keys { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }
}