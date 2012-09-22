namespace Snittlistan.Web.ViewModels.Match
{
    using System.ComponentModel.DataAnnotations;

    using Snittlistan.Web.Infrastructure.Validation;

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
            this.Player1 = new Player();
            this.Player2 = new Player();
            this.Player3 = new Player();
            this.Player4 = new Player();
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
        /// Gets or sets player 1.
        /// </summary>
        [Display(Name = "Spelare 1")]
        public Player Player1 { get; set; }

        /// <summary>
        /// Gets or sets player 2.
        /// </summary>
        [Display(Name = "Spelare 2")]
        public Player Player2 { get; set; }

        /// <summary>
        /// Gets or sets player 3.
        /// </summary>
        [Display(Name = "Spelare 3")]
        public Player Player3 { get; set; }

        /// <summary>
        /// Gets or sets player 4.
        /// </summary>
        [Display(Name = "Spelare 4")]
        public Player Player4 { get; set; }

        /// <summary>
        /// Represents a player in a match.
        /// </summary>
        public class Player
        {
            /// <summary>
            /// Initializes a new instance of the Player class.
            /// </summary>
            public Player()
            {
                this.Game1 = new Game();
                this.Game2 = new Game();
                this.Game3 = new Game();
                this.Game4 = new Game();
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
                this.Player = string.Empty;
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