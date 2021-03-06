﻿namespace LolSharp.RiotObjects.Stats
{
    public class PlayerStatsSummaryDto
    {
        public AggregatedStatsDto AggregatedStats { get; set; }
        public int Losses { get; set; }
        public long ModifyDate { get; set; }
        public string PlayerStatSummaryType { get; set; }
        public int Wins { get; set; }
    }
}