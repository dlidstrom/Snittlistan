namespace Snittlistan.Models
{
    using System.Collections.Generic;

    public abstract class TeamFactory
    {
        public List<Serie> Series { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }

        public abstract Team CreateTeam();
    }
}