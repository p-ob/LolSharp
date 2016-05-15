using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotObjects.Summoner
{
    public class MasteryPagesDto
    {
        public List<MasteryPageDto> Pages { get; set; }
        public long SummonerId { get; set; }
    }
}
