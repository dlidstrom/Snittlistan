
using System.ComponentModel;

namespace Snittlistan.Queue.WindowsServiceHost;
[RunInstaller(true)]
public partial class ProjectInstaller : System.Configuration.Install.Installer
{
    public ProjectInstaller()
    {
        InitializeComponent();
    }
}
