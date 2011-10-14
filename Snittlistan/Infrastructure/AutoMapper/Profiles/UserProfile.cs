using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public class UserProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<User, UserViewModel>()
				.ForMember(vm => vm.Name, o => o.MapFrom(m => string.Format("{0} {1}", m.FirstName, m.LastName)));
			Mapper.CreateMap<User, EditUserViewModel>()
				.ForMember(vm => vm.Password, o => o.Ignore());
		}
	}
}