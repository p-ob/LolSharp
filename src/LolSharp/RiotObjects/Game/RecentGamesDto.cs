namespace LolSharp.RiotObjects.Game
{
    using System.Collections.Generic;

    public class RecentGamesDto
    {
        public List<GameDto> Games { get; set; } 
        public long SummonerId { get; set; }
    }
}