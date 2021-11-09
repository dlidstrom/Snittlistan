#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class RegisterBitsResult
    {
        [Required]
        public string RosterId { get; set; } = null!;

        public int TeamScore { get; set; }

        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }

        [Required]
        public ResultSeriesReadModel.Serie Serie1 { get; set; } = null!;

        public ResultSeriesReadModel.Serie? Serie2 { get; set; }

        public ResultSeriesReadModel.Serie? Serie3 { get; set; }

        public ResultSeriesReadModel.Serie? Serie4 { get; set; }
    }
}
