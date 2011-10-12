using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Snittlistan.Models;

namespace Snittlistan.Infrastructure.Indexes
{
	public class Matches_PlayerStats : AbstractIndexCreationTask<Match, Matches_PlayerStats.Results>
	{
		public Matches_PlayerStats()
		{
			Map = matches => from match in matches
							 from team in match.Teams
							 from serie in team.Series
							 from table in serie.Tables
							 from game in table.Games
							 select new
							 {
								 Player = game.Player,
								 TotalPins = game.Pins,
								 Count = 1,
								 Max = game.Pins,
								 Min = game.Pins
							 };

			Reduce = results => from result in results
								group result by result.Player into stat
								select new
								{
									Player = stat.Key,
									TotalPins = stat.Sum(s => s.TotalPins),
									Count = stat.Sum(s => s.Count),
									Max = stat.Max(s => s.Max),
									Min = stat.Min(s => s.Min)
								};
		}

		public class Results
		{
			public string Player { get; set; }
			public int TotalPins { get; set; }
			public int Count { get; set; }
			public int Max { get; set; }
			public int Min { get; set; }
		}
	}
}