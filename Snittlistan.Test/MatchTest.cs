using System;
using System.Collections.Generic;
using System.Globalization;
using MvcContrib.TestHelper;
using Snittlistan.Models;
using Xunit;

namespace Snittlistan.Test
{
	public class MatchTest : DbTest
	{
		private readonly Match match;

		public MatchTest()
		{
			match = new Match(
				place: "Sollentuna Bowlinghall",
				date: DateTime.ParseExact("2011-03-26", "yyyy-MM-dd", CultureInfo.InvariantCulture),
				bitsMatchId: 3003231);
			match.HomeTeam = new Team("Sollentuna Bwk", 13);
			match.AwayTeam = new Team("Fredrikshof IF", 6);
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

			Session.Store(match);
			Session.SaveChanges();
			WaitForNonStaleResults<Match>();
			match = Session.Load<Match>(match.Id);
		}

		[Fact]
		public void PinscoreForPlayer()
		{
			match.AwayTeam.PinscoreForPlayer("Peter Sjöberg").ShouldBe(787);
		}

		[Fact]
		public void PinscoreForTeam()
		{
			match.AwayTeam.Pins.ShouldBe(6216);
			match.AwayTeam.PinsFor(1).ShouldBe(1598);
			match.AwayTeam.PinsFor(2).ShouldBe(1573);
			match.AwayTeam.PinsFor(3).ShouldBe(1505);
			match.AwayTeam.PinsFor(4).ShouldBe(1540);
		}

		[Fact]
		public void LaneScores()
		{
			match.HomeTeam.Score.ShouldBe(13);
			match.AwayTeam.ScoreFor(1).ShouldBe(2);
			match.AwayTeam.ScoreFor(2).ShouldBe(2);
			match.AwayTeam.ScoreFor(3).ShouldBe(1);
			match.AwayTeam.ScoreFor(4).ShouldBe(1);
			match.AwayTeam.Score.ShouldBe(6);
		}

		[Fact]
		public void Teams()
		{
			match.HomeTeam.Name.ShouldBe("Sollentuna Bwk");
			match.AwayTeam.Name.ShouldBe("Fredrikshof IF");
		}
	}
}