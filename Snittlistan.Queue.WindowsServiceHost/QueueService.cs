using System.ServiceProcess;

namespace Snittlistan.Queue.WindowsServiceHost
{
    public partial class QueueService : ServiceBase
    {
        public QueueService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
