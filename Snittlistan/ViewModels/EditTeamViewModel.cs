namespace Snittlistan.ViewModels
{
    using System.Web.Mvc;

    public class EditTeamViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the home team.
        /// Used when updating match.
        /// </summary>
        [HiddenInput]
        public bool IsHomeTeam { get; set; }

        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        public TeamViewModel Team { get; set; }
    }
}