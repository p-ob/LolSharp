using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LolSharp.RiotObjects.Status
{
    public class Service
    {
        public List<Incident> Incidents { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
    }
}
