namespace Snittlistan.Queue.WindowsServiceHost
{
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using log4net;

    public partial class QueueService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Application application = new Application();

        public QueueService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Log.Info("Starting queue service");
                application.Start();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.GetType().ToString(), ex);
                ExitCode = 1;
            }
        }

        protected override void OnStop()
        {
            Log.Info("Stopping queue service");
            application.Stop();
        }
    }
}
