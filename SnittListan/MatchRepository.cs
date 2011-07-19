using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SnittListan.Models;

namespace SnittListan
{
	public class MatchRepository
	{
		private List<Match> matches;

		public MatchRepository()
		{
			var match = new Match(
				id: 3003231,
				place: "Sollentuna Bowlinghall",
				date: DateTime.ParseExact("2011-03-26", "yyyy-MM-dd", CultureInfo.InvariantCulture),
				series: new List<Serie>
				{
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "M. Axelsson", 202, 1),
							new Game(1, "C. Liedholm", 218, 1),
							new Game(2, "K. Persson", 172, 0),
							new Game(2, "P. Sjöberg", 220, 0),
							new Game(3, "K. Jansson", 166, 0),
							new Game(3, "H. Norbeck", 194, 0),
							new Game(4, "L. Öberg", 222, 1),
							new Game(4, "T. Jensen", 204, 1)
						}
					},
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "L. Öberg", 182, 1),
							new Game(1, "T. Jensen", 211, 1),
							new Game(2, "K. Jansson", 208, 1),
							new Game(2, "H. Norbeck", 227, 1),
							new Game(3, "K. Persson", 194, 0),
							new Game(3, "P. Sjöberg", 195, 0),
							new Game(4, "M. Axelsson", 206, 0),
							new Game(4, "C. Liedholm", 150, 0)
						}
					},
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "K. Persson", 174, 0),
							new Game(1, "P. Sjöberg", 182, 0),
							new Game(2, "M. Axelsson", 214, 1),
							new Game(2, "C. Liedholm", 176, 1),
							new Game(3, "L. Öberg", 168, 0),
							new Game(3, "T. Jensen", 199, 0),
							new Game(4, "K. Jansson", 180, 0),
							new Game(4, "H. Norbeck", 212, 0),
						}
					},
					new Serie
					{
						Games = new List<Game>
						{
							new Game(1, "K. Jansson", 189, 0),
							new Game(1, "H. Norbeck", 181, 0),
							new Game(2, "L. Öberg", 227, 0),
							new Game(2, "T. Jensen", 180, 0),
							new Game(3, "M. Axelsson", 223, 1),
							new Game(3, "C. Liedholm", 191, 1),
							new Game(4, "T. Gurell", 159, 0),
							new Game(4, "P. Sjöberg", 190, 0),
						}
					}
				});

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