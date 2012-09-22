namespace Snittlistan.Web.Models
{
    /// <summary>
    /// Simple indirection.
    /// </summary>
    public class AwayTeamFactory : TeamFactory
    {
        public override Team8x4 CreateTeam()
        {
            return Team8x4.CreateAwayTeam(this.Name, this.Score, this.Series);
        }
    }
}