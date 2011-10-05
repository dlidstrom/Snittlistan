using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Snittlistan.ViewModels
{
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