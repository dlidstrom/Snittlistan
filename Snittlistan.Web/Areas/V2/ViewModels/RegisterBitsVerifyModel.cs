
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class RegisterBitsVerifyModel
{
    [Required]
    public string RosterId { get; set; } = null!;

    [HiddenInput]
    public int Season { get; set; }
}
