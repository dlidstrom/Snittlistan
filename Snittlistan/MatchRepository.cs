using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Snittlistan.Models;

namespace Snittlistan
{
	public class MatchRepository
	{
		private List<Match> matches;

		public MatchRepository()
		{
			var match = new Match(
				place: "Sollentuna Bowlinghall",
				date: DateTime.ParseExact("2011-03-26", "yyyy-MM-dd", CultureInfo.InvariantCulture),
				homeTeam: new Team("Sollentuna Bwk", 13),
				awayTeam: new Team("Fredrikshof IF", 6),
				bitsMatchId: 3003231);
			new List<Game>
				{
					new Game(1, 1, "Mikael Axelsson", 202, 1),
					new Game(1, 1, "Christer Liedholm", 218, 1),
					new Game(1, 2, "Kjell Persson", 172, 0),
					new Game(1, 2, "Peter Sjöberg", 220, 0),
					new Game(1, 3, "Kjell Jansson", 166, 0),
					new Game(1, 3, "Hans Norbeck", 194, 0),
					new Game(1, 4, "Lars Öberg", 222, 1),
					new Game(1, 4, "Torbjörn Jensen", 204, 1),
					new Game(2, 1, "Lars Öberg", 182, 1),
					new Game(2, 1, "Torbjörn Jensen", 211, 1),
					new Game(2, 2, "Kjell Jansson", 208, 1),
					new Game(2, 2, "Hans Norbeck", 227, 1),
					new Game(2, 3, "Kjell Persson", 194, 0),
					new Game(2, 3, "Peter Sjöberg", 195, 0),
					new Game(2, 4, "Mikael Axelsson", 206, 0),
					new Game(2, 4, "Christer Liedholm", 150, 0),
					new Game(3, 1, "Kjell Persson", 174, 0),
					new Game(3, 1, "Peter Sjöberg", 182, 0),
					new Game(3, 2, "Mikael Axelsson", 214, 1),
					new Game(3, 2, "Christer Liedholm", 176, 1),
					new Game(3, 3, "Lars Öberg", 168, 0),
					new Game(3, 3, "Torbjörn Jensen", 199, 0),
					new Game(3, 4, "Kjell Jansson", 180, 0),
					new Game(3, 4, "Hans Norbeck", 212, 0),
					new Game(4, 1, "Kjell Jansson", 189, 0),
					new Game(4, 1, "Hans Norbeck", 181, 0),
					new Game(4, 2, "Lars Öberg", 227, 0),
					new Game(4, 2, "Torbjörn Jensen", 180, 0),
					new Game(4, 3, "Mikael Axelsson", 223, 1),
					new Game(4, 3, "Christer Liedholm", 191, 1),
					new Game(4, 4, "Thomas Gurell", 159, 0),
					new Game(4, 4, "Peter Sjöberg", 190, 0),
				}.ForEach(g => match.AwayTeam.AddGame(g));

			matches = new Match[] { match }.ToList();
		}

		public Match Load(int id)
		{
			return matches.Where(m => m.Id == id).Single();
		}

		public IEnumerable<Match> LoadAll(int start, int take)
		{
			return matches.Skip(start).Take(take);
		}

		public IQueryable<Match> Query()
		{
			return matches.AsQueryable();
		}
	}
}