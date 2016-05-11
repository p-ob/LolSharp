namespace LolSharp.RiotDtos.CurrentGame
{
    using System.Collections.Generic;
    using Match;
    using Shared;

    public class CurrentGameInfo
    {
        public List<BannedChampion> BannedChampions { get; set; }
        public long GameId { get; set; }
        public long GameLength { get; set; }
        public string GameMode { get; set; }
        public long GameQueueConfigId { get; set; }
        public long GameStartTime { get; set; }
        public string GameType { get; set; }
        public long MapId { get; set; }
        public Observer Observers { get; set; }
        public List<CurrentGameParticipant> Participants { get; set; }
        public string PlatformId { get; set; }
    }
}