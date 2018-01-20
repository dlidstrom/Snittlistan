using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using log4net.Config;
using Snittlistan.Queue;
using Snittlistan.Tool.Tasks;

namespace Snittlistan.Tool
{
    public static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            Log.Info("Starting");
            try
            {
                Run(args);
            }
            finally
            {
                Log.Info("Done");
            }
        }

        private static void Run(string[] args)
        {

            IWindsorContainer container = new WindsorContainer();
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn<ICommandLineTask>()
                       .Configure(x => x.LifeStyle.Transient.Named(x.Implementation.Name.Replace("CommandLineTask", string.Empty)))
                       .WithServiceFromInterface(typeof(ICommandLineTask)));

            if (args.Length < 1)
            {
                Usage(container);
                return;
            }

            try
            {
                MsmqGateway.Initialize(ConfigurationManager.AppSettings["TaskQueue"]);
                var task = container.Resolve<ICommandLineTask>(args[0]);
                task.Run(args);
            }
            catch (Exception e)
            {
                Log.Error(e.GetType().ToString(), e);
            }
        }

        private static void Usage(IWindsorContainer container)
        {
            Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} <task>");
            Console.WriteLine();
            Console.WriteLine("Available tasks:");
            var names = container.Kernel
                                 .GetAssignableHandlers(typeof(ICommandLineTask))
                                 .Select(x => x.ComponentModel.ComponentName.Name)
                                 .OrderBy(x => x)
                                 .ToArray();
            foreach (var name in names)
            {
                Console.WriteLine("{0,30}", name);
            }

            Console.WriteLine();
            Console.WriteLine("To get help, use: " + AppDomain.CurrentDomain.FriendlyName + " help <task>");
        }
    }
}
