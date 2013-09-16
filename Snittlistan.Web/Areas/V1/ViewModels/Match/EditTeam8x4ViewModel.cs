using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V1.ViewModels.Match
{
    public class EditTeam8x4ViewModel
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
        public Team8x4ViewModel Team { get; set; }
    }
}