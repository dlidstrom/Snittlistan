using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Snittlistan.Web.Infrastructure.Indexes;

namespace Snittlistan.Web.Areas.V1.ViewModels.Match
{
    public class Match8x4ViewModel
    {
        public MatchDetails Match { get; set; }

        public Team8x4DetailsViewModel HomeTeam { get; set; }

        public List<Player_ByMatch.Result> HomeTeamResults { get; set; }

        public Team8x4DetailsViewModel AwayTeam { get; set; }

        public List<Player_ByMatch.Result> AwayTeamResults { get; set; }

        public class MatchDetails
        {
            public MatchDetails()
            {
                Date = DateTimeOffset.Now.Date;
            }

            [HiddenInput]
            public int Id { get; set; }

            [Required(ErrorMessage = "Ange BITS matchid")]
            [Display(Name = "BITS MatchId")]
            public int BitsMatchId { get; set; }

            [Required(ErrorMessage = "Ange plats")]
            [Display(Name = "Plats")]
            public string Location { get; set; }

            [Required(ErrorMessage = "Ange datum")]
            [Display(Name = "Datum"), DataType(DataType.Date)]
            public DateTimeOffset Date { get; set; }
        }
    }
}