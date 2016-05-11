﻿namespace LolSharp.RiotDtos.Match
{
    public class ParticipantFrame
    {
        public int CurrentGold { get; set; }
        public int DominionScore { get; set; }
        public int JungleMinionsKilled { get; set; }
        public int Level { get; set; }
        public int MinionsKilled { get; set; }
        public int ParticipantId { get; set; }
        public Position Position { get; set; }
        public int TeamScore { get; set; }
        public int TotalGold { get; set; }
        public int Xp { get; set; }
    }
}