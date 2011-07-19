using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnittListan.Models
{
	public class Game
	{
		public Game(int table, string player, int pinScore, int laneScore)
		{
			Table = table;
			Player = player;
			PinScore = pinScore;
			LaneScore = laneScore;
		}

		public int Table { get; private set; }
		public string Player { get; private set; }
		public int PinScore { get; private set; }
		public int LaneScore { get; private set; }

		public int Strikes { get; private set; }
		public int Misses { get; private set; }
		public int OnePinMisses { get; private set; }
		public int Splits { get; private set; }
		public bool NothingLeft { get; private set; }
	}
}