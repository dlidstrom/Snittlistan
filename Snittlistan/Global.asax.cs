using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Common.Logging;
using Raven.Client;
using Snittlistan.Helpers;
using Snittlistan.Helpers.Attributes;
using Snittlistan.Infrastructure;
using Snittlistan.Infrastructure.AutoMapper;
using Snittlistan.Infrastructure.Indexes;
using Snittlistan.IoC;
using Snittlistan.Models;

namespace Snittlistan
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();
        private static IWindsorContainer container;

        public static IWindsorContainer Container
        {
            get { return container; }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandleErrorAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RavenActionFilterAttribute());
        }

        protected void Application_Start()
        {
            logger.Info("Application Starting");
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            // initialize container and controller factory
            InitializeContainer();

            // register routes
            new RouteConfigurator(RouteTable.Routes).Configure();

            // add model binders
            ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

            // configure AutoMapper
            AutoMapperConfiguration.Configure(container);

#if DEBUG
            using (var session = Container.Resolve<IDocumentStore>().OpenSession())
            {
                var match = session.Query<Match>().SingleOrDefault(m => m.BitsMatchId == 3003231);
                if (match == null)
                {
                    session.Store(DbSeed.CreateMatch());
                }

                session.SaveChanges();
            }
#endif
        }

        protected void Application_End()
        {
            logger.Info("Application Ending");
            container.Dispose();
        }

        private void InitializeContainer()
        {
            if (container == null)
            {
                container = new WindsorContainer()
                    .Install(FromAssembly.This());
            }

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}