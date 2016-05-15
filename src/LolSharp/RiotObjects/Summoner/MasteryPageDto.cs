namespace LolSharp.RiotObjects.Summoner
{
    using System.Collections.Generic;

    public class MasteryPageDto
    {
        public bool Current { get; set; }
        public long Id { get; set; }
        public List<MasteryDto> Masteries { get; set; }
        public string Name { get; set; }
    }
}
