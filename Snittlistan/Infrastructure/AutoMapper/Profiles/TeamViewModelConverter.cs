namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using System.Collections.Generic;
    using Snittlistan.Models;
    using Snittlistan.ViewModels;

    public class TeamViewModelConverter<TFactory> : global::AutoMapper.ITypeConverter<TeamViewModel, TFactory>
        where TFactory : TeamFactory, new()
    {
        public TFactory Convert(global::AutoMapper.ResolutionContext context)
        {
            var vm = (TeamViewModel)context.SourceValue;
            return new TFactory
            {
                Name = vm.Name,
                Score = vm.Score,
                Series = new List<Serie>
                {
                    new Serie(new List<Table>
                    {
                        vm.Pair1.Serie1.MapTo<Table>(),
                        vm.Pair2.Serie1.MapTo<Table>(),
                        vm.Pair3.Serie1.MapTo<Table>(),
                        vm.Pair4.Serie1.MapTo<Table>()
                    }),
                    new Serie(new List<Table>
                    {
                        vm.Pair1.Serie2.MapTo<Table>(),
                        vm.Pair2.Serie2.MapTo<Table>(),
                        vm.Pair3.Serie2.MapTo<Table>(),
                        vm.Pair4.Serie2.MapTo<Table>()
                    }),
                    new Serie(new List<Table>
                    {
                        vm.Pair1.Serie3.MapTo<Table>(),
                        vm.Pair2.Serie3.MapTo<Table>(),
                        vm.Pair3.Serie3.MapTo<Table>(),
                        vm.Pair4.Serie3.MapTo<Table>()
                    }),
                    new Serie(new List<Table>
                    {
                        vm.Pair1.Serie4.MapTo<Table>(),
                        vm.Pair2.Serie4.MapTo<Table>(),
                        vm.Pair3.Serie4.MapTo<Table>(),
                        vm.Pair4.Serie4.MapTo<Table>()
                    }),
                }
            };
        }
    }
}