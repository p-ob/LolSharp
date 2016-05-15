namespace LolSharp.RiotObjects.Summoner
{
    using System.Collections.Generic;

    public class RunePagesDto
    {
        public List<RunePageDto> Pages { get; set; }
        public long SummonerId { get; set; }
    }
}
