using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Absence
    {
        public Absence(DateTime from, DateTime to, string player)
        {
            From = from;
            To = to;
            Player = player;
        }

        public int Id { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Player { get; set; }
    }
}