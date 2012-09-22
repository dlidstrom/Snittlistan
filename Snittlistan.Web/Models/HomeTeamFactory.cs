namespace Snittlistan.Web.Models
{
    /// <summary>
    /// Simple indirection.
    /// </summary>
    public class HomeTeamFactory : TeamFactory
    {
        public override Team8x4 CreateTeam()
        {
            return Team8x4.CreateHomeTeam(this.Name, this.Score, this.Series);
        }
    }
}