using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snittlistan.ViewModels
{
	public class TeamSummaryViewModel
	{
		[Display(Name = "Namn")]
		public string Name { get; set; }

		[Display(Name = "Banpoäng")]
		public int Score { get; set; }

        public List<Serie> Series { get; set; }

        public class Serie
        {
            public List<Table> Tables { get; set; }
        }

        public class Table
        {
            public int Score { get; set; }
            public int Total { get; set; }
            public Game Game1 { get; set; }
            public Game Game2 { get; set; }
        }

        public class Game
        {
            public string Player { get; set; }
            public int Pins { get; set; }
        }
	}
}