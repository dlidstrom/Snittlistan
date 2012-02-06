namespace Snittlistan.Models
{
    using System.Collections.Generic;

    public abstract class TeamFactory
    {
        public List<Serie8x4> Series { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }

        public abstract Team8x4 CreateTeam();
    }
}