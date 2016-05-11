using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotDtos.League
{
    public class LeagueDto
    {
        public List<LeagueEntryDto> Entries { get; set; }
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public string Queue { get; set; }
        public string Tier { get; set; }
    }
}
