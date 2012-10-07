namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Models;

    public class RosterProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            global::AutoMapper.Mapper.CreateMap<Roster, RosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(m => m.Date.TimeOfDay.ToString()));
        }
    }
}