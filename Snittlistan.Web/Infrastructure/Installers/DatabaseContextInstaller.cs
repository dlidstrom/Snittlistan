#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Npgsql.Logging;
    using Snittlistan.Queue.Infrastructure;

    public class DatabaseContextInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            NpgsqlLogManager.Provider = new NLogLoggingProvider();
            NpgsqlLogManager.IsParameterLoggingEnabled = true;

            _ = container.Register(
                Component.For<Database.DatabaseContext>()
                    .UsingFactoryMethod(_ => new Database.DatabaseContext())
                    .LifestylePerWebRequest());
        }
    }
}
