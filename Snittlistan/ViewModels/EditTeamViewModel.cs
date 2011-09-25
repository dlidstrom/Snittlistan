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

		[HiddenInput]
		public bool IsHomeTeam { get; set; }

		public TeamViewModel Team { get; set; }
	}
}