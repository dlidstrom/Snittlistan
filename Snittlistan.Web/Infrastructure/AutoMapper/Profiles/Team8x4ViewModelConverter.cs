namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using System.Collections.Generic;

    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Match;

    public class Team8x4ViewModelConverter<TFactory> : global::AutoMapper.ITypeConverter<Team8x4ViewModel, TFactory>
        where TFactory : TeamFactory, new()
    {
        public TFactory Convert(global::AutoMapper.ResolutionContext context)
        {
            var vm = (Team8x4ViewModel)context.SourceValue;
            return new TFactory
            {
                Name = vm.Name,
                Score = vm.Score,
                Series = new List<Serie8x4>
                {
                    new Serie8x4(new List<Table8x4>
                    {
                        vm.Pair1.Serie1.MapTo<Table8x4>(),
                        vm.Pair2.Serie1.MapTo<Table8x4>(),
                        vm.Pair3.Serie1.MapTo<Table8x4>(),
                        vm.Pair4.Serie1.MapTo<Table8x4>()
                    }),
                    new Serie8x4(new List<Table8x4>
                    {
                        vm.Pair1.Serie2.MapTo<Table8x4>(),
                        vm.Pair2.Serie2.MapTo<Table8x4>(),
                        vm.Pair3.Serie2.MapTo<Table8x4>(),
                        vm.Pair4.Serie2.MapTo<Table8x4>()
                    }),
                    new Serie8x4(new List<Table8x4>
                    {
                        vm.Pair1.Serie3.MapTo<Table8x4>(),
                        vm.Pair2.Serie3.MapTo<Table8x4>(),
                        vm.Pair3.Serie3.MapTo<Table8x4>(),
                        vm.Pair4.Serie3.MapTo<Table8x4>()
                    }),
                    new Serie8x4(new List<Table8x4>
                    {
                        vm.Pair1.Serie4.MapTo<Table8x4>(),
                        vm.Pair2.Serie4.MapTo<Table8x4>(),
                        vm.Pair3.Serie4.MapTo<Table8x4>(),
                        vm.Pair4.Serie4.MapTo<Table8x4>()
                    }),
                }
            };
        }
    }
}