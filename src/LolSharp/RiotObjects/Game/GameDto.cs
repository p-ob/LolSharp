﻿namespace LolSharp.RiotObjects.Game
{
    using System.Collections.Generic;

    public class GameDto
    {
        public int ChampionId { get; set; }
        public long CreateDate { get; set; }
        public List<PlayerDto> FellowPlayers { get; set; }
        public long GameId { get; set; }
        public string GameMode { get; set; }
        public string GameType { get; set; }
        public bool Invalid { get; set; }
        public int IpEarned { get; set; }
        public int Level { get; set; }
        public int MapId { get; set; }
        public int Spell1 { get; set; }
        public int Spell2 { get; set; }
        public RawStatsDto Stats { get; set; }
        public string SubType { get; set; }
        public int TeamId { get; set; }
    }
}