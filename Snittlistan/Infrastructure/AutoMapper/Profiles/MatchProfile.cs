namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using Snittlistan.Models;
    using Snittlistan.ViewModels;

    public class MatchProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            // model -> viewmodel
            global::AutoMapper.Mapper.CreateMap<Match, MatchViewModel.MatchDetails>();
            global::AutoMapper.Mapper.CreateMap<Team, TeamDetailsViewModel>();
            global::AutoMapper.Mapper.CreateMap<Serie, TeamDetailsViewModel.Serie>();
            global::AutoMapper.Mapper.CreateMap<Table, TeamDetailsViewModel.Table>()
                .ForMember(vm => vm.Total, o => o.ResolveUsing(m => m.Game1.Pins + m.Game2.Pins));
            global::AutoMapper.Mapper.CreateMap<Game, TeamDetailsViewModel.Game>()
                .ForMember(vm => vm.Player, o => o.ResolveUsing<NameShortenerResolver>());
            global::AutoMapper.Mapper.CreateMap<Team, TeamViewModel>()
                .ForMember(vm => vm.Pair1, o => o.ResolveUsing(new PairResolver { Pair = 0 }))
                .ForMember(vm => vm.Pair2, o => o.ResolveUsing(new PairResolver { Pair = 1 }))
                .ForMember(vm => vm.Pair3, o => o.ResolveUsing(new PairResolver { Pair = 2 }))
                .ForMember(vm => vm.Pair4, o => o.ResolveUsing(new PairResolver { Pair = 3 }));
            global::AutoMapper.Mapper.CreateMap<Table, TeamViewModel.Serie>();
            global::AutoMapper.Mapper.CreateMap<Game, TeamViewModel.Game>();

            // viewmodel -> model
            global::AutoMapper.Mapper.CreateMap<TeamViewModel, HomeTeamFactory>()
                .ConvertUsing<TeamViewModelConverter<HomeTeamFactory>>();
            global::AutoMapper.Mapper.CreateMap<TeamViewModel, AwayTeamFactory>()
                .ConvertUsing<TeamViewModelConverter<AwayTeamFactory>>();
            global::AutoMapper.Mapper.CreateMap<TeamViewModel.Serie, Table>();
            global::AutoMapper.Mapper.CreateMap<TeamViewModel.Game, Game>();
        }
    }
}