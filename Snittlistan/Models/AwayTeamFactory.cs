namespace Snittlistan.Models
{
	/// <summary>
	/// Simple indirection.
	/// </summary>
	public class AwayTeamFactory : TeamFactory
	{
		public override Team CreateTeam()
		{
			return Team.CreateAwayTeam(Name, Score, Series);
		}
	}
}