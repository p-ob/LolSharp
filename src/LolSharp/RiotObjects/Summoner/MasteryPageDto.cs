using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotObjects.Summoner
{
    public class MasteryPageDto
    {
        public bool Current { get; set; }
        public long Id { get; set; }
        public List<MasteryDto> Masteries { get; set; }
        public string Name { get; set; }
    }
}
