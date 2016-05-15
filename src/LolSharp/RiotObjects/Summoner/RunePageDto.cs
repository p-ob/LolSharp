namespace LolSharp.RiotObjects.Summoner
{
    using System.Collections.Generic;

    public class RunePageDto
    {
        public bool Current { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<RuneSlotDto> Slots { get; set; }
    }
}
