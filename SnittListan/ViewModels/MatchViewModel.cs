namespace SnittListan.ViewModels
{
	public class MatchViewModel
	{
		public string Id { get; set; }
		public string Place { get; set; }
		public string Date { get; set; }
		public string Results { get; set; }
		public string Teams { get; set; }
		public Game[] Games { get; set; }

		public class Game
		{
			public string Player { get; set; }
			public int PinScore { get; set; }
			public int LaneScore { get; set; }
		}
	}
}