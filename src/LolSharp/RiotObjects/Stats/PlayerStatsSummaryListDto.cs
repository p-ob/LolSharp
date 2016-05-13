namespace LolSharp.RiotObjects.Stats
{
    using System.Collections.Generic;

    public class PlayerStatsSummaryListDto
    {
        public List<PlayerStatsSummaryDto> PlayerStatSummaries { get; set; }
        public long SummonerId { get; set; }  
    }
}