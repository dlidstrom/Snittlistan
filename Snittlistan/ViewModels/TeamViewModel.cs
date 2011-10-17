using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.ViewModels
{
	/// <summary>
	/// Represents a team.
	/// </summary>
	public class TeamViewModel
	{
		/// <summary>
		/// Initializes a new instance of the TeamViewModel class.
		/// </summary>
		public TeamViewModel()
		{
			Pair1 = new Pair();
			Pair2 = new Pair();
			Pair3 = new Pair();
			Pair4 = new Pair();
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[Display(Name = "Namn")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the score.
		/// </summary>
		[Display(Name = "Banpoäng"), Range(0, 20)]
		public int Score { get; set; }

		/// <summary>
		/// Gets or sets the first pair.
		/// </summary>
		[Display(Name = "Par 1")]
		public Pair Pair1 { get; set; }

		/// <summary>
		/// Gets or sets the second pair.
		/// </summary>
		[Display(Name = "Par 2")]
		public Pair Pair2 { get; set; }

		/// <summary>
		/// Gets or sets the third pair.
		/// </summary>
		[Display(Name = "Par 3")]
		public Pair Pair3 { get; set; }

		/// <summary>
		/// Gets or sets the fourth pair.
		/// </summary>
		[Display(Name = "Par 4")]
		public Pair Pair4 { get; set; }

		/// <summary>
		/// Represents a pair in a match.
		/// </summary>
		public class Pair
		{
			/// <summary>
			/// Initializes a new instance of the Pair class.
			/// </summary>
			public Pair()
			{
				Serie1 = new Serie();
				Serie2 = new Serie();
				Serie3 = new Serie();
				Serie4 = new Serie();
			}

			/// <summary>
			/// Gets or sets serie 1.
			/// </summary>
			[Display(Name = "1")]
			public Serie Serie1 { get; set; }

			/// <summary>
			/// Gets or sets serie 2.
			/// </summary>
			[Display(Name = "2")]
			public Serie Serie2 { get; set; }

			/// <summary>
			/// Gets or sets serie 3.
			/// </summary>
			[Display(Name = "3")]
			public Serie Serie3 { get; set; }

			/// <summary>
			/// Gets or sets serie 4.
			/// </summary>
			[Display(Name = "4")]
			public Serie Serie4 { get; set; }
		}

		/// <summary>
		/// Represents a serie by a pair.
		/// </summary>
		public class Serie
		{
			/// <summary>
			/// Initializes a new instance of the Serie class.
			/// </summary>
			public Serie()
			{
				Game1 = new Game();
				Game2 = new Game();
			}

			/// <summary>
			/// Gets or sets the score.
			/// </summary>
			[Display(Name = "Banpoäng"), Range(0, 1)]
			public int Score { get; set; }

			/// <summary>
			/// Gets or sets the first game.
			/// </summary>
			public Game Game1 { get; set; }

			/// <summary>
			/// Gets or sets the second game.
			/// </summary>
			public Game Game2 { get; set; }
		}

		/// <summary>
		/// Represents a game in a serie.
		/// </summary>
		public class Game
		{
			/// <summary>
			/// Initializes a new instance of the Game class.
			/// </summary>
			public Game()
			{
				Player = string.Empty;
			}

			/// <summary>
			/// Gets or sets the player name.
			/// </summary>
			[Display(Name = "Spelare")]
			public string Player { get; set; }

			/// <summary>
			/// Gets or sets the number of pins.
			/// </summary>
			[Display(Name = "Kägelpoäng"), Range(0, 300)]
			public int Pins { get; set; }

			/// <summary>
			/// Gets or sets the number of strikes.
			/// </summary>
			[Display(Name = "X"), Range(0, 12)]
			public int Strikes { get; set; }

			/// <summary>
			/// Gets or sets the number of misses.
			/// </summary>
			[Display(Name = "Miss"), Range(0, 12)]
			public int Misses { get; set; }

			/// <summary>
			/// Gets or sets the number of one-pin misses.
			/// </summary>
			[Display(Name = "9-"), Range(0, 12)]
			public int OnePinMisses { get; set; }

			/// <summary>
			/// Gets or sets the number of splits.
			/// </summary>
			[Display(Name = "Hål"), Range(0, 12)]
			public int Splits { get; set; }

			/// <summary>
			/// Gets or sets a value indicating whether all frames were covered.
			/// </summary>
			[Display(Name = "Alla täckta")]
			public bool CoveredAll { get; set; }
		}
	}
}