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
			Series = new List<Serie> { new Serie() };
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		[Required, Display(Name = "Namn")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the score.
		/// </summary>
		[Required, Display(Name = "Banpoäng"), Range(0, 20)]
		public int Score { get; set; }

		/// <summary>
		/// Gets or sets the series.
		/// </summary>
		public List<Serie> Series { get; set; }

		/// <summary>
		/// Represents a serie in a match.
		/// </summary>
		public class Serie
		{
			/// <summary>
			/// Initializes a new instance of the Serie class.
			/// </summary>
			public Serie()
			{
				Tables = new List<Table> { new Table() };
			}

			/// <summary>
			/// Gets or sets the tables.
			/// </summary>
			public List<Table> Tables { get; set; }
		}

		/// <summary>
		/// Represents a table in a serie.
		/// </summary>
		public class Table
		{
			/// <summary>
			/// Initializes a new instance of the Table class.
			/// </summary>
			public Table()
			{
				FirstGame = new Game();
				SecondGame = new Game();
			}

			/// <summary>
			/// Gets or sets the score.
			/// </summary>
			[Display(Name = "Banpoäng")]
			public int Score { get; set; }

			/// <summary>
			/// Gets or sets the first game.
			/// </summary>
			public Game FirstGame { get; set; }

			/// <summary>
			/// Gets or sets the second game.
			/// </summary>
			public Game SecondGame { get; set; }
		}
		
		/// <summary>
		/// Represents a game in a table.
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
			[Required, Display(Name = "Spelare")]
			public string Player { get; set; }

			/// <summary>
			/// Gets or sets the number of pins.
			/// </summary>
			[Display(Name = "Kägelpoäng")]
			public int Pins { get; set; }

			/// <summary>
			/// Gets or sets the number of strikes.
			/// </summary>
			[Display(Name = "X")]
			public int Strikes { get; set; }

			/// <summary>
			/// Gets or sets the number of misses.
			/// </summary>
			[Display(Name = "Miss")]
			public int Misses { get; set; }

			/// <summary>
			/// Gets or sets the number of one-pin misses.
			/// </summary>
			[Display(Name = "9-")]
			public int OnePinMisses { get; set; }

			/// <summary>
			/// Gets or sets the number of splits.
			/// </summary>
			[Display(Name = "Hål")]
			public int Splits { get; set; }

			/// <summary>
			/// Gets or sets a value indicating whether all frames were covered.
			/// </summary>
			[Display(Name = "Alla täckta")]
			public bool CoveredAll { get; set; }
		}
	}
}