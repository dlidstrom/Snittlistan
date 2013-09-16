using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V1.ViewModels.Match
{
    /// <summary>
    /// Represents the view used to register a match.
    /// </summary>
    public class Register8x4MatchViewModel
    {
        /// <summary>
        /// Initializes a new instance of the Register8x4MatchViewModel class.
        /// </summary>
        public Register8x4MatchViewModel()
        {
            Date = DateTime.Now;
            Location = string.Empty;
            HomeTeam = new Team8x4ViewModel();
            AwayTeam = new Team8x4ViewModel();
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
        /// Gets or sets the bits match id.
        /// </summary>
        [Required(ErrorMessage = "Ange BITS matchid")]
        [Remote("IsBitsMatchIdAvailable", "Validation", ErrorMessage = "Matchen är redan registrerad")]
        [Display(Name = "BITS MatchId")]
        public int BitsMatchId { get; set; }

        /// <summary>
        /// Gets or sets the home team.
        /// </summary>
        [Display(Name = "Hemmalag")]
        public Team8x4ViewModel HomeTeam { get; set; }

        /// <summary>
        /// Gets or sets the away team.
        /// </summary>
        [Display(Name = "Bortalag")]
        public Team8x4ViewModel AwayTeam { get; set; }
    }
}