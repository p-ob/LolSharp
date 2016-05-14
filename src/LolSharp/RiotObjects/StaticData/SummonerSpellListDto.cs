namespace LolSharp.RiotObjects.StaticData
{
    using System.Collections.Generic;

    public class SummonerSpellListDto
    {
        public Dictionary<string, SummonerSpellDto> Data { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }
}
