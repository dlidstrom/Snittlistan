namespace Snittlistan.Queue.ConsoleHost
{
    using System;
    using log4net.Config;

    public class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            Console.WriteLine("Press [ENTER] to start.");
            Console.ReadLine();
            var application = new Application();
            application.Start();
            Console.WriteLine("Press [ENTER] to stop.");
            Console.ReadLine();
            application.Stop();
        }
    }
}
