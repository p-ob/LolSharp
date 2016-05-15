namespace LolSharp.RiotObjects.Status
{
    using System.Collections.Generic;

    public class Incident
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public string CreatedAt { get; set; }
        public long Id { get; set; }
        public string Severity { get; set; }
        public List<Translation> Translations { get; set; }
        public string UpdatedAt { get; set; }
    }
}
