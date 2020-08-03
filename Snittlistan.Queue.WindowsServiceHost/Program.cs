namespace Snittlistan.Queue.WindowsServiceHost
{
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using log4net;
    using log4net.Config;

    public static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main()
        {
            XmlConfigurator.Configure();
            try
            {
                Log.Info("Starting queue service host");
                Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex.GetType().ToString(), ex);
                Environment.ExitCode = 1;
            }

            Log.Info("Stopping queue service host");
        }

        private static void Run()
        {
            var servicesToRun = new ServiceBase[]
            {
                new QueueService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
