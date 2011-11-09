namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using Snittlistan.Models;
    using Snittlistan.ViewModels;

    public class UserProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            global::AutoMapper.Mapper.CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.Name, o => o.MapFrom(m => string.Format("{0} {1}", m.FirstName, m.LastName)));
            global::AutoMapper.Mapper.CreateMap<User, EditUserViewModel>()
                .ForMember(vm => vm.Password, o => o.Ignore());
        }
    }
}