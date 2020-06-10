namespace Snittlistan.Queue.WindowsServiceHost
{
    using System.ServiceProcess;
    using log4net.Config;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            XmlConfigurator.Configure();
            var servicesToRun = new ServiceBase[]
            {
                new QueueService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
