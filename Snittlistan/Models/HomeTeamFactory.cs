namespace Snittlistan.Models
{
	/// <summary>
	/// Simple indirection.
	/// </summary>
	public class HomeTeamFactory : TeamFactory
	{
		public override Team CreateTeam()
		{
			return Team.CreateHomeTeam(Name, Score, Series);
		}
	}
}