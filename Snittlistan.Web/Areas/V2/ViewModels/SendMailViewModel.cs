#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Areas.V2.ViewModels;

public class SendMailViewModel
{
    public SendMailViewModel()
    {
        Recipient = string.Empty;
        ReplyTo = string.Empty;
        Subject = string.Empty;
        Content = string.Empty;
    }

    [Required]
    public string Recipient { get; set; }

    [Required]
    public string ReplyTo { get; set; }

    [Required, StringLength(128)]
    public string Subject { get; set; }

    [Required, StringLength(1024)]
    public string Content { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public RateSettingValues? RateSetting { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public MailCountValues? MailCount { get; set; }

    public enum RateSettingValues
    {
        [Display(Name = "1 per minut")]
        OnePerMinute = 1,
        [Display(Name = "1 var 5:e minut")]
        OnePerFiveMinutes,
        [Display(Name = "1 var 15:e minut")]
        OnePerFifteenMinutes
    }

    public enum MailCountValues
    {
        [Display(Name = "1 mail")]
        One = 1,
        [Display(Name = "5 mail")]
        Five,
        [Display(Name = "15 mail")]
        Fifteen
    }
}
