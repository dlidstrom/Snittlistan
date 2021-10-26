#nullable enable

namespace Snittlistan.Tool
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NLog;
    using Npgsql.Logging;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Infrastructure;
    using Snittlistan.Tool.Tasks;

    public static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("Starting");
            NpgsqlLogManager.Provider = new NLogLoggingProvider();
            NpgsqlLogManager.IsParameterLoggingEnabled = true;
            try
            {
                Run(args);
            }
            finally
            {
                Logger.Info("Done");
            }
        }

        private static void Run(string[] args)
        {
            IWindsorContainer container = new WindsorContainer();
            _ = container.Register(
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
                ICommandLineTask task = container.Resolve<ICommandLineTask>(args[0]);
                task.Run(args);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Unhandled exception");
            }
        }

        private static void Usage(IWindsorContainer container)
        {
            Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} <task>");
            Console.WriteLine();
            Console.WriteLine("Available tasks:");
            string[] names = container.Kernel
                                 .GetAssignableHandlers(typeof(ICommandLineTask))
                                 .Select(x => x.ComponentModel.ComponentName.Name)
                                 .OrderBy(x => x)
                                 .ToArray();
            foreach (string name in names)
            {
                Console.WriteLine("{0,30}", name);
            }

            Console.WriteLine();
            Console.WriteLine("To get help, use: " + AppDomain.CurrentDomain.FriendlyName + " help <task>");
        }
    }
}
