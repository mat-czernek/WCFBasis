using System.ComponentModel;
using System.Configuration.Install;

namespace ConcreteService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void WCFServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
