using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    public class MatchProfile : Profile
    {
        protected override void Configure()
        {
            // model -> viewmodel
            Mapper.CreateMap<Match, MatchViewModel.MatchDetails>();
            Mapper.CreateMap<Team, TeamDetailsViewModel>();
            Mapper.CreateMap<Serie, TeamDetailsViewModel.Serie>();
            Mapper.CreateMap<Table, TeamDetailsViewModel.Table>()
                .ForMember(vm => vm.Total, o => o.ResolveUsing(m => m.Game1.Pins + m.Game2.Pins));
            Mapper.CreateMap<Game, TeamDetailsViewModel.Game>();
            Mapper.CreateMap<Team, TeamViewModel>()
                .ForMember(vm => vm.Pair1, o => o.ResolveUsing(new PairResolver { Pair = 0 }))
                .ForMember(vm => vm.Pair2, o => o.ResolveUsing(new PairResolver { Pair = 1 }))
                .ForMember(vm => vm.Pair3, o => o.ResolveUsing(new PairResolver { Pair = 2 }))
                .ForMember(vm => vm.Pair4, o => o.ResolveUsing(new PairResolver { Pair = 3 }));
            Mapper.CreateMap<Table, TeamViewModel.Serie>();
            Mapper.CreateMap<Game, TeamViewModel.Game>();

            // viewmodel -> model
            Mapper.CreateMap<TeamViewModel, HomeTeamFactory>()
                .ConvertUsing<TeamViewModelConverter<HomeTeamFactory>>();
            Mapper.CreateMap<TeamViewModel, AwayTeamFactory>()
                .ConvertUsing<TeamViewModelConverter<AwayTeamFactory>>();
            Mapper.CreateMap<TeamViewModel.Serie, Table>();
            Mapper.CreateMap<TeamViewModel.Game, Game>();
        }
    }
}