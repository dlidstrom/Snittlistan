using AutoMapper;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ViewModels;

namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    public class RosterProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Roster, RosterViewModel>()
                .ForMember(vm => vm.Time, o => o.MapFrom(m => m.Date.ToShortTimeString()))
                .ForMember(vm => vm.Players, o => o.Ignore())
                .ForMember(vm => vm.TeamLeader, o => o.Ignore());

            Mapper.CreateMap<Roster, CreateRosterViewModel>();
        }
    }
}