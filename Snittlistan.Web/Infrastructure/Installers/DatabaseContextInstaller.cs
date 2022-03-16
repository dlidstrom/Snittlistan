#nullable enable

using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure.Installers;

public class DatabaseContextInstaller : IWindsorInstaller
{
    private readonly Func<Databases> databasesFactory;

    private readonly Func<ComponentRegistration<Databases>, ComponentRegistration<Databases>> func;

    public DatabaseContextInstaller(Func<Databases> databasesFactory)
    {
        func = x => x.LifestylePerWebRequest();
        this.databasesFactory = databasesFactory;
    }

    public DatabaseContextInstaller(Func<Databases> databasesFactory, LifestyleType lifestyleType)
    {
        func = x => x.LifeStyle.Is(lifestyleType);
        this.databasesFactory = databasesFactory;
    }

    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        _ = container.Register(
            func.Invoke(
                Component.For<Databases>()
                    .UsingFactoryMethod(databasesFactory)));
        _ = container.Register(
                Component.For<DatabasesFactory>()
                    .Instance(new DatabasesFactory(databasesFactory))
                    .LifestyleSingleton());
    }
}
