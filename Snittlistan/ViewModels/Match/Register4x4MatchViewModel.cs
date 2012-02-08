namespace Snittlistan.ViewModels.Match
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Register4x4MatchViewModel
    {
        /// <summary>
        /// Initializes a new instance of the Register4x4MatchViewModel class.
        /// </summary>
        public Register4x4MatchViewModel()
        {
            Date = DateTimeOffset.Now;
            Location = string.Empty;
            HomeTeam = new Team4x4ViewModel();
            AwayTeam = new Team4x4ViewModel();
        }

        /// <summary>
        /// Gets or sets the match date.
        /// </summary>
        [Required(ErrorMessage = "Ange datum")]
        [Display(Name = "Datum"), DataType(DataType.Date)]
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the match location.
        /// </summary>
        [Required(ErrorMessage = "Ange plats")]
        [Display(Name = "Plats")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the home team.
        /// </summary>
        [Display(Name = "Hemmalag")]
        public Team4x4ViewModel HomeTeam { get; set; }

        /// <summary>
        /// Gets or sets the away team.
        /// </summary>
        [Display(Name = "Bortalag")]
        public Team4x4ViewModel AwayTeam { get; set; }
    }
}