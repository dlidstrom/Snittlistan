namespace Snittlistan.Infrastructure.AutoMapper
{
    using Castle.Windsor;

    public static class AutoMapperConfiguration
    {
        public static void Configure(IWindsorContainer container)
        {
            var handlers = container.ResolveAll<global::AutoMapper.Profile>();
            foreach (var handler in handlers)
            {
                global::AutoMapper.Mapper.AddProfile(handler);
            }
        }
    }
}