namespace LolSharp.RiotDtos.Stats
{
    using System.Collections.Generic;

    public class RankedStatsDto
    {
        public List<ChampionStatsDto> Champions { get; set; }
        public long ModifyDate { get; set; }
        public long SummonerId { get; set; } 
    }
}
