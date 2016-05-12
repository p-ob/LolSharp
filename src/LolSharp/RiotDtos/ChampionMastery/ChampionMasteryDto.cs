namespace LolSharp.RiotDtos.ChampionMastery
{
    public class ChampionMasteryDto
    {
        public long ChampionId { get; set; } 
        public int ChampionLevel { get; set; }
        public int ChampionPoints { get; set; }
        public long ChampionPointsSinceLastLevel { get; set; }
        public long ChampionPointsUntilNextLevel { get; set; }
        public bool ChestGranted { get; set; }
        public string HighestGrade { get; set; }
        public long LastPlayTime { get; set; }
        public long PlayerId { get; set; }
    }
}