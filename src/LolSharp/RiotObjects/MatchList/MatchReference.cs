namespace LolSharp.RiotObjects.MatchList
{
    public class MatchReference
    {
        public long Champion { get; set; }
        public string Lane { get; set; }
        public long MatchId { get; set; }
        public string PlatformId { get; set; }
        public string Queue { get; set; }
        public string Region { get; set; }
        public string Role { get; set; }
        public string Season { get; set; }
        public long Timestamp { get; set; }
    }
}
