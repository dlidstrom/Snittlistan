using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snittlistan.Models
{
	public class Table
	{
		public int Score { get; set; }
		public Game FirstGame { get; set; }
		public Game SecondGame { get; set; }

		public int Pins
		{
			get
			{
				return FirstGame.Pins + SecondGame.Pins;
			}
		}

		public int PinscoreForPlayer(string player)
		{
			if (FirstGame.Player == player)
				return FirstGame.Pins;
			else if (SecondGame.Player == player)
				return SecondGame.Pins;
			else
				return 0;
		}
	}
}
