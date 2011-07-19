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
				.ForMember(x => x.Date, o => o.MapFrom(y => y.Date.ToShortDateString()));
		}
	}
}