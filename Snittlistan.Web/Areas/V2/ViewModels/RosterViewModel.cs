namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RosterViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

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
                if (this.Team.Length < 1) throw new InvalidOperationException("Initialize Team first");
                return char.ToLower(this.Team[this.Team.Length - 1]);
            }
        }
    }
}