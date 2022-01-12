
using System.ComponentModel.DataAnnotations;
using Snittlistan.Web.Areas.V2.ReadModels;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class RegisterBitsResult4
{
    [Required]
    public string RosterId { get; set; } = null!;

    public int TeamScore { get; set; }

    public int OpponentScore { get; set; }

    public int BitsMatchId { get; set; }

    [Required]
    public ResultSeries4ReadModel.Serie Serie1 { get; set; } = null!;

    public ResultSeries4ReadModel.Serie? Serie2 { get; set; }

    public ResultSeries4ReadModel.Serie? Serie3 { get; set; }

    public ResultSeries4ReadModel.Serie? Serie4 { get; set; }
}
