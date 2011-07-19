using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnittListan.Models
{
	public class Game
	{
		public Game(int table, string player, int total, int score)
		{
			Table = table;
			Player = player;
			Total = total;
			Score = score;
		}

		public int Table { get; set; }
		public string Player { get; private set; }
		public int Total { get; private set; }
		public int Score { get; set; }

		public int Strikes { get; private set; }
		public int Misses { get; private set; }
		public int OnePinMisses { get; private set; }
		public int Splits { get; private set; }
		public bool NothingLeft { get; private set; }
	}
}