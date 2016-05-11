namespace LolSharp.RiotDtos.Match
{
    using System.Collections.Generic;

    public class Timeline
    {
        public long FrameInterval { get; set; }
        public List<Frame> Frames { get; set; }
    }
}
