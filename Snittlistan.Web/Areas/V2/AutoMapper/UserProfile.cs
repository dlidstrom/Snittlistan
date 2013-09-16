using AutoMapper;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.AutoMapper
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserViewModel>();
        }
    }
}