namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;

    public class Match4x4Profile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            // model -> viewmodel
            global::AutoMapper.Mapper.CreateMap<Match4x4, Match4x4ViewModel.MatchDetails>();
            global::AutoMapper.Mapper.CreateMap<Team4x4, Team4x4DetailsViewModel>();
            global::AutoMapper.Mapper.CreateMap<Serie4x4, Team4x4DetailsViewModel.Serie>();
            global::AutoMapper.Mapper.CreateMap<Game4x4, Team4x4DetailsViewModel.Game>();
            global::AutoMapper.Mapper.CreateMap<Team4x4, Team4x4ViewModel>()
                .ForMember(vm => vm.Serie1, o => o.ResolveUsing(new SerieResolver { Serie = 0 }))
                .ForMember(vm => vm.Serie2, o => o.ResolveUsing(new SerieResolver { Serie = 1 }))
                .ForMember(vm => vm.Serie3, o => o.ResolveUsing(new SerieResolver { Serie = 2 }))
                .ForMember(vm => vm.Serie4, o => o.ResolveUsing(new SerieResolver { Serie = 3 }));
            global::AutoMapper.Mapper.CreateMap<Game4x4, Team4x4ViewModel.Game>();

            // viewmodel -> model
            global::AutoMapper.Mapper.CreateMap<Match4x4ViewModel.MatchDetails, Match4x4>()
                .ForMember(vm => vm.HomeTeam, o => o.Ignore())
                .ForMember(vm => vm.AwayTeam, o => o.Ignore());
            global::AutoMapper.Mapper.CreateMap<Team4x4ViewModel, Team4x4>()
                .ConvertUsing<Team4x4ViewModelConverter>();
            global::AutoMapper.Mapper.CreateMap<Team4x4ViewModel.Serie, Serie4x4>();
            global::AutoMapper.Mapper.CreateMap<Team4x4ViewModel.Game, Game4x4>();
        }
    }
}