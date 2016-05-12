namespace LolSharp.RiotDtos.Match
{
    using System.Collections.Generic;

    public class Frame
    {
        public List<Event> Events { get; set; }
        public Dictionary<string, ParticipantFrame> ParticipantFrames { get; set; }
        public long Timestamp { get; set; }
    }
}