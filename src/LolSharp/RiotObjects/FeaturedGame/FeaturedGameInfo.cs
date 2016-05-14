namespace LolSharp.RiotObjects.FeaturedGame
{
    using Match;
    using Shared;
    using System.Collections.Generic;

    public class FeaturedGameInfo
    {
        public List<BannedChampion> BannedChampions { get; set; }
        public long GameId { get; set; }
        public long GameLength { get; set; }
        public string GameMode { get; set; }
        public long GameQueueConfigId { get; set; }
        public long GameStartTime { get; set; }
        public string GameType { get; set; }
        public Observer Observers { get; set; }
        public List<Participant> Participants { get; set; }
        public string PlatformId { get; set; }
    }
}
