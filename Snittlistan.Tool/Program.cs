using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NLog;
using Snittlistan.Queue;
using Snittlistan.Tool.Tasks;

#nullable enable

namespace Snittlistan.Tool;
public static class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static async Task Main(string[] args)
    {
        Logger.Info("Starting");
        try
        {
            await Run(args);
        }
        finally
        {
            Logger.Info("Done");
        }
    }

    private static async Task Run(string[] args)
    {
        IWindsorContainer container = new WindsorContainer();
        HttpConnectionSettings settings = new(
            ConfigurationManager.AppSettings["UrlScheme"],
            Convert.ToInt32(ConfigurationManager.AppSettings["Port"]));
        _ = container.Register(Component.For<HttpConnectionSettings>().Instance(settings));
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
            await task.Run(args);
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
        int indent = names.Max(x => x.Length);
        foreach (string name in names)
        {
            Console.WriteLine("{0," + indent + "}", name);
        }

        Console.WriteLine();
        Console.WriteLine("To get help, use: " + AppDomain.CurrentDomain.FriendlyName + " help <task>");
    }
}
