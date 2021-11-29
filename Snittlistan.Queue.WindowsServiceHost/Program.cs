#nullable enable

namespace Snittlistan.Queue.WindowsServiceHost
{
    using System;
    using System.ServiceProcess;
    using NLog;

    public static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            try
            {
                Logger.Info("Starting queue service host");
                Run();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Environment.ExitCode = 1;
            }

            Logger.Info("Stopping queue service host");
        }

        private static void Run()
        {
            ServiceBase[] servicesToRun = new[]
            {
                new QueueService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
