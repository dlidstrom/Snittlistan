#nullable enable

namespace Snittlistan.Queue.ConsoleHost
{
    using System;
    using System.Configuration;
    using Npgsql.Logging;
    using Snittlistan.Queue.Config;
    using Snittlistan.Queue.Infrastructure;

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Press [ENTER] to start.");
            _ = Console.ReadLine();
            NpgsqlLogManager.Provider = new NLogLoggingProvider();
            NpgsqlLogManager.IsParameterLoggingEnabled = true;
            Application application = new(
                (MessagingConfigSection)ConfigurationManager.GetSection("messaging"),
                ConfigurationManager.AppSettings["UrlScheme"]);
            application.Start();
            Console.WriteLine("Press [ENTER] to stop.");
            _ = Console.ReadLine();
            application.Stop();
        }
    }
}
