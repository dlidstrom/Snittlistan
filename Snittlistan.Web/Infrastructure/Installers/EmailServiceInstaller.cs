#nullable enable

using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Postal;

namespace Snittlistan.Web.Infrastructure.Installers;

public class EmailServiceInstaller : IWindsorInstaller
{
    private readonly string viewsPath;

    public EmailServiceInstaller(string viewsPath)
    {
        this.viewsPath = viewsPath;
    }

    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        ViewEngineCollection engines = new()
        {
            new FileSystemRazorViewEngine(viewsPath)
        };
        _ = container.Register(Component.For<IEmailService>().Instance(new EmailService(engines)));
    }
}
