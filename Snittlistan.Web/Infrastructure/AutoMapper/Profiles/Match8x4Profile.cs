using Snittlistan.Web.Areas.V1.Models;
using Snittlistan.Web.Areas.V1.ViewModels.Match;

namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    public class Match8x4Profile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            // model -> viewmodel
            global::AutoMapper.Mapper.CreateMap<Match8x4, Match8x4ViewModel.MatchDetails>();
            global::AutoMapper.Mapper.CreateMap<Team8x4, Team8x4DetailsViewModel>();
            global::AutoMapper.Mapper.CreateMap<Serie8x4, Team8x4DetailsViewModel.Serie>();
            global::AutoMapper.Mapper.CreateMap<Table8x4, Team8x4DetailsViewModel.Table>()
                .ForMember(vm => vm.Total, o => o.ResolveUsing(m => m.Game1.Pins + m.Game2.Pins));
            global::AutoMapper.Mapper.CreateMap<Game8x4, Team8x4DetailsViewModel.Game>()
                .ForMember(vm => vm.Player, o => o.MapFrom(g => NameShortener.Shorten(g.Player)));
            global::AutoMapper.Mapper.CreateMap<Team8x4, Team8x4ViewModel>()
                .ForMember(vm => vm.Pair1, o => o.ResolveUsing(new PairResolver { Pair = 0 }))
                .ForMember(vm => vm.Pair2, o => o.ResolveUsing(new PairResolver { Pair = 1 }))
                .ForMember(vm => vm.Pair3, o => o.ResolveUsing(new PairResolver { Pair = 2 }))
                .ForMember(vm => vm.Pair4, o => o.ResolveUsing(new PairResolver { Pair = 3 }));
            global::AutoMapper.Mapper.CreateMap<Table8x4, Team8x4ViewModel.Serie>();
            global::AutoMapper.Mapper.CreateMap<Game8x4, Team8x4ViewModel.Game>();

            // viewmodel -> model
            global::AutoMapper.Mapper.CreateMap<Team8x4ViewModel, HomeTeamFactory>()
                .ConvertUsing<Team8x4ViewModelConverter<HomeTeamFactory>>();
            global::AutoMapper.Mapper.CreateMap<Team8x4ViewModel, AwayTeamFactory>()
                .ConvertUsing<Team8x4ViewModelConverter<AwayTeamFactory>>();
            global::AutoMapper.Mapper.CreateMap<Team8x4ViewModel.Serie, Table8x4>();
            global::AutoMapper.Mapper.CreateMap<Team8x4ViewModel.Game, Game8x4>();
        }
    }
}