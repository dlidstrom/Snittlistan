#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Snittlistan.Web.Areas.V2.ViewModels;

public class SendMailViewModel
{
    public SendMailViewModel()
    {
        Recipient = string.Empty;
        Subject = string.Empty;
        Content = string.Empty;
    }

    [Required]
    public string Recipient { get; set; }

    [Required, StringLength(128)]
    public string Subject { get; set; }

    [Required, StringLength(1024)]
    public string Content { get; set; }

    public enum RateSetting
    {
        OnePerMinute,
        OnePerFiveMinutes,
        OnePerFifteenMinutes
    }
}
