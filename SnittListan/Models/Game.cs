using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnittListan.Models
{
	public class Game
	{
		public Game(int serie, int table, string player, int pins, int score)
		{
			Serie = serie;
			Table = table;
			Player = player;
			Pins = pins;
			Score = score;
		}

		public int Serie { get; private set; }
		public int Table { get; private set; }
		public string Player { get; private set; }
		public int Pins { get; private set; }
		public int Score { get; private set; }

		public int Strikes { get; private set; }
		public int Misses { get; private set; }
		public int OnePinMisses { get; private set; }
		public int Splits { get; private set; }
		public bool CoveredAll { get; private set; }
	}
}