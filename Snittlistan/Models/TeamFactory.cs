using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snittlistan.Models
{
	public abstract class TeamFactory
	{
		public List<Serie> Series { get; set; }
		public string Name { get; set; }
		public int Score { get; set; }

		public abstract Team CreateTeam();
	}
}