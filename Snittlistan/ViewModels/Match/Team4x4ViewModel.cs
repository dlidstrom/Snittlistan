namespace Snittlistan.ViewModels.Match
{
    using System.ComponentModel.DataAnnotations;
    using Snittlistan.Infrastructure.Validation;

    /// <summary>
    /// Represents a team.
    /// </summary>
    public class Team4x4ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the Team4x4ViewModel class.
        /// </summary>
        public Team4x4ViewModel()
        {
            Serie1 = new Serie();
            Serie2 = new Serie();
            Serie3 = new Serie();
            Serie4 = new Serie();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "Ange namn")]
        [Display(Name = "Namn")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        [Required(ErrorMessage = "Ange banpoäng")]
        [Display(Name = "Banpoäng"), Range(0, 20)]
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets serie 1.
        /// </summary>
        [Display(Name = "Serie 1")]
        public Serie Serie1 { get; set; }

        /// <summary>
        /// Gets or sets serie 2.
        /// </summary>
        [Display(Name = "Serie 2")]
        public Serie Serie2 { get; set; }

        /// <summary>
        /// Gets or sets serie 3.
        /// </summary>
        [Display(Name = "Serie 3")]
        public Serie Serie3 { get; set; }

        /// <summary>
        /// Gets or sets serie 4.
        /// </summary>
        [Display(Name = "Serie 4")]
        public Serie Serie4 { get; set; }

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
                Game1 = new Game();
                Game2 = new Game();
                Game3 = new Game();
                Game4 = new Game();
            }

            /// <summary>
            /// Gets or sets the first game.
            /// </summary>
            public Game Game1 { get; set; }

            /// <summary>
            /// Gets or sets the second game.
            /// </summary>
            public Game Game2 { get; set; }

            /// <summary>
            /// Gets or sets the third game.
            /// </summary>
            public Game Game3 { get; set; }

            /// <summary>
            /// Gets or sets the fourth game.
            /// </summary>
            public Game Game4 { get; set; }
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
            /// Gets or sets the score.
            /// </summary>
            [Display(Name = "Banpoäng"), Range(0, 1)]
            public int Score { get; set; }

            /// <summary>
            /// Gets or sets the number of pins.
            /// </summary>
            [Display(Name = "Kägelpoäng"), Range(0, 300)]
            public int Pins { get; set; }

            /// <summary>
            /// Gets or sets the number of strikes.
            /// </summary>
            [Display(Name = "X"), Range(0, 12)]
            public int? Strikes { get; set; }

            /// <summary>
            /// Gets or sets the number of misses.
            /// </summary>
            [Display(Name = "Miss"), Range(0, 12)]
            [RequiredIfExists("Strikes")]
            public int? Misses { get; set; }

            /// <summary>
            /// Gets or sets the number of one-pin misses.
            /// </summary>
            [Display(Name = "9-"), Range(0, 12)]
            [RequiredIfExists("Strikes")]
            public int? OnePinMisses { get; set; }

            /// <summary>
            /// Gets or sets the number of splits.
            /// </summary>
            [Display(Name = "Hål"), Range(0, 12)]
            [RequiredIfExists("Strikes")]
            public int? Splits { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether all frames were covered.
            /// </summary>
            [Display(Name = "Alla täckta")]
            public bool CoveredAll { get; set; }
        }
    }
}