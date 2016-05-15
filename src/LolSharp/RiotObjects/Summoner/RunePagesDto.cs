using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotObjects.Summoner
{
    public class RunePagesDto
    {
        public List<RunePageDto> Pages { get; set; }
        public long SummonerId { get; set; }
    }
}
