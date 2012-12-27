namespace Snittlistan.Web.Areas.V2.AutoMapper
{
    using global::AutoMapper;
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Models;

    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserViewModel>();
        }
    }
}