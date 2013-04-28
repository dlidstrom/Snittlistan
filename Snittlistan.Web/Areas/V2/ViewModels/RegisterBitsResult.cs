using System.ComponentModel.DataAnnotations;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RegisterBitsResult
    {
        [Required]
        public string RosterId { get; set; }

        public int TeamScore { get; set; }

        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }

        [Required]
        public ResultSeriesReadModel.Serie Serie1 { get; set; }

        public ResultSeriesReadModel.Serie Serie2 { get; set; }

        public ResultSeriesReadModel.Serie Serie3 { get; set; }

        public ResultSeriesReadModel.Serie Serie4 { get; set; }
    }
}