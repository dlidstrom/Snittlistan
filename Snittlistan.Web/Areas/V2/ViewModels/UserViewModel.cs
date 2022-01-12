
using System.ComponentModel.DataAnnotations;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class UserViewModel
{
    public UserViewModel(User user)
    {
        Id = user.Id!;
        Name = $"{user.FirstName} {user.LastName}";
        Email = user.Email;
        IsActive = user.IsActive;
    }

    public string Id { get; set; }

    [Display(Name = "Namn")]
    public string Name { get; set; }

    [Display(Name = "E-postadress")]
    public string Email { get; set; }

    [Display(Name = "Aktiv")]
    public bool IsActive { get; set; }
}
