namespace LolSharp.RiotDtos.MatchList
{
    using System.Collections.Generic;

    public class MatchList
    {
        public int EndIndex { get; set; }
        public List<MatchReference> Matches { get; set; } 
        public int StartIndex { get; set; }
        public int TotalGames { get; set; }
    }
}
