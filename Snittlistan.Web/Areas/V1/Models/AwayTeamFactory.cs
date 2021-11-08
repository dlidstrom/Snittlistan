#nullable enable

namespace Snittlistan.Web.Areas.V1.Models
{
    /// <summary>
    /// Simple indirection.
    /// </summary>
    public class AwayTeamFactory : TeamFactory
    {
        public override Team8x4 CreateTeam()
        {
            return Team8x4.CreateAwayTeam(Name!, Score, Series!);
        }
    }
}
