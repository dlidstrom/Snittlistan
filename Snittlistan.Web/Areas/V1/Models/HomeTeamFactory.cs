namespace Snittlistan.Web.Areas.V1.Models
{
    /// <summary>
    /// Simple indirection.
    /// </summary>
    public class HomeTeamFactory : TeamFactory
    {
        public override Team8x4 CreateTeam()
        {
            return Team8x4.CreateHomeTeam(Name, Score, Series);
        }
    }
}