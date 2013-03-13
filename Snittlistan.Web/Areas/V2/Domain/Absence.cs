using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class Absence
    {
        public Absence(DateTime from, DateTime to, string player)
        {
            this.From = from;
            this.To = to;
            this.Player = player;
        }

        public int Id { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Player { get; set; }
    }
}