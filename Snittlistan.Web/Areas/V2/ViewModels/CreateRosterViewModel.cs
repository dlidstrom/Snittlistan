using System;
using System.ComponentModel.DataAnnotations;
using Raven.Abstractions;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class CreateRosterViewModel
    {
        public CreateRosterViewModel()
        {
            Team = string.Empty;
            Location = string.Empty;
            Opponent = string.Empty;
            Date = SystemTime.UtcNow.ToLocalTime().Date.AddHours(10);
        }

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

        public bool IsFourPlayer { get; set; }
    }
}