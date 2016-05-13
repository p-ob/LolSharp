namespace LolSharp.RiotObjects.League
{
    using System.Collections.Generic;

    public class LeagueDto
    {
        public List<LeagueEntryDto> Entries { get; set; }
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public string Queue { get; set; }
        public string Tier { get; set; }
    }
}
