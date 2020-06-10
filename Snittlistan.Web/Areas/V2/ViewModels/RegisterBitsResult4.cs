namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class RegisterBitsResult4
    {
        [Required]
        public string RosterId { get; set; }

        public int TeamScore { get; set; }

        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }

        [Required]
        public ResultSeries4ReadModel.Serie Serie1 { get; set; }

        public ResultSeries4ReadModel.Serie Serie2 { get; set; }

        public ResultSeries4ReadModel.Serie Serie3 { get; set; }

        public ResultSeries4ReadModel.Serie Serie4 { get; set; }
    }
}