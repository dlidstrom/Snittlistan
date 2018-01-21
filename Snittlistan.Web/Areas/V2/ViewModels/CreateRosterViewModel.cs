using System;
using System.ComponentModel.DataAnnotations;
using Raven.Abstractions;
using Snittlistan.Web.Areas.V2.Domain;

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

        public CreateRosterViewModel(Roster roster)
        {
            Season = roster.Season;
            Turn = roster.Turn;
            BitsMatchId = roster.BitsMatchId;
            Team = roster.Team;
            Location = roster.Location;
            Opponent = roster.Opponent;
            Date = roster.Date;
            IsFourPlayer = roster.IsFourPlayer;
            OilPatternName = roster.OilPattern.Name;
            OilPatternUrl = roster.OilPattern.Url;
        }

        [Required]
        public int Season { get; set; }

        [Required]
        public int Turn { get; set; }

        [Required]
        public int BitsMatchId { get; set; }

        [Required]
        public string Team { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Opponent { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool IsFourPlayer { get; set; }

        public string OilPatternName { get; set; }

        public string OilPatternUrl { get; set; }
    }
}