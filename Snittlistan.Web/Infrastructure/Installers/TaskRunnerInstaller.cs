using System;
using System.Collections.Generic;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NLog;
using Raven.Client;
using Snittlistan.Web.Infrastructure.BackgroundTasks;

namespace Snittlistan.Web.Infrastructure.Installers
{
    public class TaskRunnerInstaller : IWindsorInstaller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var value = ConfigurationManager.AppSettings["TaskRunnerPollingInterval"];
            var taskRunnerPollingInterval = Convert.ToInt32(value);
            if (taskRunnerPollingInterval < 10000)
            {
                Log.Warn("Invalid TaskRunnerPollingInterval: {0}", value);
                taskRunnerPollingInterval = 10000;
            }

            var handledDatabases = new HashSet<string>();
            var tenantConfigurations = container.ResolveAll<TenantConfiguration>();
            foreach (var tenantConfiguration in tenantConfigurations)
            {
                if (handledDatabases.Contains(tenantConfiguration.Database))
                {
                    continue;
                }

                var key = $"DocumentStore-{tenantConfiguration.Name}";
                var documentStore = container.Resolve<IDocumentStore>(key);
                var taskRunner = new TaskRunner(
                    container.Kernel,
                    documentStore,
                    taskRunnerPollingInterval);
                container.Register(
                    Component.For<TaskRunner>()
                             .Instance(taskRunner)
                             .Named($"TaskRunner-{tenantConfiguration.Name}")
                             .LifestyleSingleton());

                handledDatabases.Add(tenantConfiguration.Database);
            }
        }
    }
}