#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Npgsql.Logging;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.Infrastructure.Logging;

    public class DatabaseContextInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            NpgsqlLogManager.Provider = new NLogLoggingProvider();
            NpgsqlLogManager.IsParameterLoggingEnabled = true;

            _ = container.Register(
                Component.For<DatabaseContext>()
                    .UsingFactoryMethod(_ => new DatabaseContext())
                    .LifestylePerWebRequest());
        }
    }
}
