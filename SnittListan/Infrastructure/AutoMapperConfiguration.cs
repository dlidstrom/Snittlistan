using AutoMapper;
using SnittListan.Models;
using SnittListan.ViewModels;

namespace SnittListan.Infrastructure
{
	public static class AutoMapperConfiguration
	{
		public static void Configure()
		{
			Mapper.CreateMap<Match, MatchViewModel>();
		}
	}
}