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
				bitsMatchId: 3003231,
				homeTeam: new Team("Sollentuna Bwk", 13),
				awayTeam: new Team("Fredrikshof IF", 6));
			var series = new List<Serie>
				{
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 1,
								FirstGame = new Game("Mikael Axelsson", 202),
								SecondGame = new Game("Christer Liedholm", 218)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Kjell Persson", 172),
								SecondGame = new Game("Peter Sjöberg", 220),
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Kjell Jansson", 166),
								SecondGame = new Game("Hans Norbeck", 194)
							},
							new Table
							{
								Score = 1,
								FirstGame = new Game("Lars Öberg", 222),
								SecondGame = new Game("Torbjörn Jensen", 204)
							}
						}
					},
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 1,
								FirstGame = new Game("Lars Öberg", 182),
								SecondGame = new Game("Torbjörn Jensen", 211)
							},
							new Table
							{
								Score = 1,
								FirstGame = new Game("Kjell Jansson", 208),
								SecondGame = new Game("Hans Norbeck", 227)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Kjell Persson", 194),
								SecondGame = new Game("Peter Sjöberg", 195)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Mikael Axelsson", 206),
								SecondGame = new Game("Christer Liedholm", 150)
							}
						}
					},
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 0,
								FirstGame = new Game("Kjell Persson", 174),
								SecondGame = new Game("Peter Sjöberg", 182)
							},
							new Table
							{
								Score = 1,
								FirstGame = new Game("Mikael Axelsson", 214),
								SecondGame = new Game("Christer Liedholm", 176)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Lars Öberg", 168),
								SecondGame = new Game("Torbjörn Jensen", 199)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Kjell Jansson", 180),
								SecondGame = new Game("Hans Norbeck", 212)
							}
						}
					},
					new Serie
					{
						Tables = new List<Table>
						{
							new Table
							{
								Score = 0,
								FirstGame = new Game("Kjell Jansson", 189),
								SecondGame = new Game("Hans Norbeck", 181)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Lars Öberg", 227),
								SecondGame = new Game("Torbjörn Jensen", 180)
							},
							new Table
							{
								Score = 1,
								FirstGame = new Game("Mikael Axelsson", 223),
								SecondGame = new Game("Christer Liedholm", 191)
							},
							new Table
							{
								Score = 0,
								FirstGame = new Game("Thomas Gurell", 159),
								SecondGame = new Game("Peter Sjöberg", 190)
							}
						}
					}
				};
			match.AwayTeam.Series = series;

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