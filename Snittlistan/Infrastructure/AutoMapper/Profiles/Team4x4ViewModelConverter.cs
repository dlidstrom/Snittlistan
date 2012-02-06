namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using System.Collections.Generic;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;

    public class Team4x4ViewModelConverter : global::AutoMapper.ITypeConverter<Team4x4ViewModel, Team4x4>
    {
        public Team4x4 Convert(global::AutoMapper.ResolutionContext context)
        {
            var vm = (Team4x4ViewModel)context.SourceValue;
            return new Team4x4(
                name: vm.Name,
                score: vm.Score,
                series: new List<Serie4x4>
                {
                    new Serie4x4(new List<Game4x4>
                    {
                        vm.Serie1.Game1.MapTo<Game4x4>(),
                        vm.Serie1.Game2.MapTo<Game4x4>(),
                        vm.Serie1.Game3.MapTo<Game4x4>(),
                        vm.Serie1.Game4.MapTo<Game4x4>()
                    }),
                    new Serie4x4(new List<Game4x4>
                    {
                        vm.Serie2.Game1.MapTo<Game4x4>(),
                        vm.Serie2.Game2.MapTo<Game4x4>(),
                        vm.Serie2.Game3.MapTo<Game4x4>(),
                        vm.Serie2.Game4.MapTo<Game4x4>()
                    }),
                    new Serie4x4(new List<Game4x4>
                    {
                        vm.Serie3.Game1.MapTo<Game4x4>(),
                        vm.Serie3.Game2.MapTo<Game4x4>(),
                        vm.Serie3.Game3.MapTo<Game4x4>(),
                        vm.Serie3.Game4.MapTo<Game4x4>()
                    }),
                    new Serie4x4(new List<Game4x4>
                    {
                        vm.Serie4.Game1.MapTo<Game4x4>(),
                        vm.Serie4.Game2.MapTo<Game4x4>(),
                        vm.Serie4.Game3.MapTo<Game4x4>(),
                        vm.Serie4.Game4.MapTo<Game4x4>()
                    })
                });
        }
    }
}