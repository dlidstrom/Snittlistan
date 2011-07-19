using AutoMapper;
using Castle.Windsor;

namespace SnittListan.Infrastructure
{
	public static class AutoMapperConfiguration
	{
		public static void Configure(IWindsorContainer container)
		{
			var handlers = container.ResolveAll<Profile>();
			foreach (var handler in handlers)
			{
				Mapper.AddProfile(handler);
			}
		}
	}
}