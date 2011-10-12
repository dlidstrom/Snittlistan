using AutoMapper;
using Castle.Windsor;

namespace Snittlistan.Infrastructure.AutoMapper
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