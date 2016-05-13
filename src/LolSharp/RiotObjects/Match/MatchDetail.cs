namespace LolSharp.RiotObjects.Match
{
    using System.Collections.Generic;

    public class MatchDetail
    {
        public int MapId { get; set; }
        public long MatchCreation { get; set; }
        public long MatchDuration { get; set; }
        public long MatchId { get; set; }
        public string MatchMode { get; set; }
        public string MatchType { get; set; }
        public string MatchVersion { get; set; }
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }
        public List<Participant> Participants { get; set; }
        public string PlatformId { get; set; }
        public string QueueType { get; set; }
        public string Region { get; set; }
        public string Season { get; set; }
        public List<Team> Teams { get; set; }
        public Timeline Timeline { get; set; }
    }
}
