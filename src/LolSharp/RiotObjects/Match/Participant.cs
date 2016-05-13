namespace LolSharp.RiotObjects.Match
{
    using System.Collections.Generic;
    using Shared;

    public class Participant
    {
        public int ChampionId { get; set; }
        public string HighestAchievedSeasonTier { get; set; }
        public List<Mastery> Masteries { get; set; }
        public int ParticipantId { get; set; }
        public List<Rune> Runes { get; set; }
        public int Spell1Id { get; set; }
        public int Spell2Id { get; set; }
        public ParticipantStats Stats { get; set; }
        public int TeamId { get; set; }
        public ParticipantTimeline Timeline { get; set; }

    }
}
