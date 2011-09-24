using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.ViewModels
{
	public class TeamViewModel
	{
		public TeamViewModel()
		{
			Series = new List<Serie> { new Serie() };
		}

		[HiddenInput]
		public int Id { get; set; }

		[Required, Display(Name = "Namn")]
		public string Name { get; set; }

		[Required, Display(Name = "Banpoäng"), Range(0, 20)]
		public int Score { get; set; }

		public List<Serie> Series { get; set; }

		public class Serie
		{
			public Serie()
			{
				Tables = new List<Table> { new Table() };
			}

			public List<Table> Tables { get; set; }
		}

		public class Table
		{
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

			public Game FirstGame { get; set; }
			public Game SecondGame { get; set; }
		}
		
		public class Game
		{
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
