namespace Snittlistan.Models
{
	public class Game
	{
		public Game(string player, int pins)
		{
			Player = player;
			Pins = pins;
		}

		public string Player { get; private set; }
		public int Pins { get; private set; }

		public int Strikes { get; private set; }
		public int Misses { get; private set; }
		public int OnePinMisses { get; private set; }
		public int Splits { get; private set; }
		public bool CoveredAll { get; private set; }
	}
}