namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using global::AutoMapper;

    using Snittlistan.Web.Areas.V2.Models;
    using Snittlistan.Web.Areas.V2.ViewModels;

    public class RosterProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Roster, RosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(m => m.Date.ToShortTimeString()))
                .ForMember(vm => vm.Players, o => o.Ignore());

            Mapper.CreateMap<Roster, CreateRosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(r => r.Date.ToShortTimeString()));
        }
    }
}