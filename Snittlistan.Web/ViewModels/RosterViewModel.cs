namespace Snittlistan.Web.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RosterViewModel
    {
        [Required]
        public int Season { get; set; }

        [Required]
        public int Turn { get; set; }

        [Required]
        public string Team { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Opponent { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time.")]
        public string Time { get; set; }

        public char TeamLevel
        {
            get
            {
                if (Team.Length < 1) throw new InvalidOperationException("Initialize Team first");
                return char.ToLower(Team[Team.Length - 1]);
            }
        }
    }
}