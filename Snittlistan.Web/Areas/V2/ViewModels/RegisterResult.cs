using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RegisterResult
    {
        public RegisterResult()
        {
        }

        public RegisterResult(ResultHeaderReadModel matchResult)
        {
            AggregateId = matchResult.AggregateId;
            TeamScore = matchResult.TeamScore;
            OpponentScore = matchResult.OpponentScore;
        }

        [HiddenInput]
        public string AggregateId { get; set; }

        [Required]
        public string RosterId { get; set; }

        [Range(0, 20), Required]
        public int? TeamScore { get; set; }

        [Range(0, 20), Required]
        public int? OpponentScore { get; set; }

        [Required]
        public int? BitsMatchId { get; set; }
    }
}