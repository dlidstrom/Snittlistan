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
								 Pins = game.Pins,
								 Series = 1,
								 Max = game.Pins,
								 Min = game.Pins,
								 Strikes = game.Strikes,
								 Misses = game.Misses,
								 OnePinMisses = game.OnePinMisses,
								 Splits = game.Splits
							 };

			Reduce = results => from result in results
								group result by result.Player into stat
								select new
								{
									Player = stat.Key,
									Pins = stat.Sum(s => s.Pins),
									Series = stat.Sum(s => s.Series),
									Max = stat.Max(s => s.Max),
									Min = stat.Min(s => s.Min),
									Strikes = stat.Sum(s => s.Strikes),
									Misses = stat.Sum(s => s.Misses),
									OnePinMisses = stat.Sum(s => s.OnePinMisses),
									Splits = stat.Sum(s => s.Splits)
								};
		}

		public class Results
		{
			public string Player { get; set; }
			public int Pins { get; set; }
			public int Series { get; set; }
			public int Max { get; set; }
			public int Min { get; set; }
			public int Strikes { get; set; }
			public int Misses { get; set; }
			public int OnePinMisses { get; set; }
			public int Splits { get; set; }
		}
	}
}