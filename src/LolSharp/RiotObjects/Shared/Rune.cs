namespace LolSharp.RiotObjects.Shared
{
    // Rank and Count mean the same thing, but different endpoints return different properties. Yeah, nicely done Rito.
    public class Rune
    {
        public long Rank { get; set; }
        public long RuneId { get; set; }
        public int Count { get; set; }
    }
}
