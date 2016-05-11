namespace LolSharp.RiotDtos.StaticData
{
    using System.Collections.Generic;

    public class BlockDto
    {
        public List<BlockItemDto> Items { get; set; }
        public bool RecMath { get; set; }
        public string Type { get; set; }
    }
}