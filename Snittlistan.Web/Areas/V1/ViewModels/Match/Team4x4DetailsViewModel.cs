namespace Snittlistan.Web.Areas.V1.ViewModels.Match
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Used to display match details for a single team.
    /// </summary>
    public class Team4x4DetailsViewModel
    {
        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Banpoäng")]
        public int Score { get; set; }

        public List<Serie> Series { get; set; }

        public class Serie
        {
            public List<Game> Games { get; set; }
        }

        public class Game
        {
            public string Player { get; set; }

            public int Pins { get; set; }

            public int Score { get; set; }
        }
    }
}