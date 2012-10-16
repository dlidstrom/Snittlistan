namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using global::AutoMapper;

    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Models;

    public class RosterProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Roster, RosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(m => m.Date.TimeOfDay.ToString()));

            Mapper.CreateMap<Roster, CreateRosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(r => r.Date.TimeOfDay.ToString()));

            Mapper.CreateMap<Roster, EditRosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(r => r.Date.TimeOfDay.ToString()));
        }
    }
}