#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Npgsql.Logging;
    using Snittlistan.Queue.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;

    public class DatabaseContextInstaller : IWindsorInstaller
    {
        private readonly Func<Databases> databases;

        public DatabaseContextInstaller(Func<Databases> databases)
        {
            this.databases = databases;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            NpgsqlLogManager.Provider = new NLogLoggingProvider();
            NpgsqlLogManager.IsParameterLoggingEnabled = true;

            _ = container.Register(
                Component.For<Databases>()
                    .UsingFactoryMethod(databases)
                    .LifestylePerWebRequest());
        }
    }
}
