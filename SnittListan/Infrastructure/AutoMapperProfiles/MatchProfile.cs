using System.Threading;
using AutoMapper;
using SnittListan.Models;
using SnittListan.ViewModels;

namespace SnittListan.Infrastructure
{
	public class MatchProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<Match, MatchViewModel>()
				.ForMember(x => x.Date, o => o.MapFrom(y => y.Date.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern, Thread.CurrentThread.CurrentCulture)))
				.ForMember(x => x.Results, o => o.MapFrom(y => y.FormattedLaneScore()))
				.ForMember(x => x.Teams, o => o.MapFrom(y => string.Format("{0}-{1}", y.HomeTeam, y.OppTeam)))
				.ForMember(x => x.Games, o => o.MapFrom(y => y.Games));

			Mapper.CreateMap<Game, MatchViewModel.Game>();
		}
	}
}