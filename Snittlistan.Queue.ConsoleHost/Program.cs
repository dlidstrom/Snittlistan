#nullable enable

using System.Configuration;
using System.Diagnostics;
using Snittlistan.Queue.Config;

namespace Snittlistan.Queue.ConsoleHost;

public class Program
{
    public static void Main()
    {
        if (Debugger.IsAttached == false)
        {
            Console.WriteLine("Press [ENTER] to start.");
            _ = Console.ReadLine();
        }

        Application application = new(
            (MessagingConfigSection)ConfigurationManager.GetSection("messaging"),
            ConfigurationManager.AppSettings["UrlScheme"],
            Convert.ToInt32(ConfigurationManager.AppSettings["Port"]));
        application.Start();
        Console.WriteLine("Press [ENTER] to stop.");
        _ = Console.ReadLine();
        application.Stop();
    }
}
