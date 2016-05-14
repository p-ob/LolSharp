namespace LolSharp.RiotObjects.StaticData
{
    using System.Collections.Generic;

    public class RuneListDto
    {
        public BasicDataDto Basic { get; set; }
        public Dictionary<string, RuneDto> Data { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }
}
