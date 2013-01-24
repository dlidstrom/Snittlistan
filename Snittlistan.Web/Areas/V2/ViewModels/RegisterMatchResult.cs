namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterMatchResult
    {
        [Required]
        public string RosterId { get; set; }

        [Range(0, 20)]
        public int TeamScore { get; set; }

        [Range(0, 20)]
        public int OpponentScore { get; set; }

        public int BitsMatchId { get; set; }
    }
}