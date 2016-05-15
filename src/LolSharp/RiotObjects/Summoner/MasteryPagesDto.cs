namespace LolSharp.RiotObjects.Summoner
{
    using System.Collections.Generic;

    public class MasteryPagesDto
    {
        public List<MasteryPageDto> Pages { get; set; }
        public long SummonerId { get; set; }
    }
}
