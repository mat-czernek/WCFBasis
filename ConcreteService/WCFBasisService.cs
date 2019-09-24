using System.ServiceProcess;
using Service;
using Service.Actions;
using Service.Services;

namespace ConcreteService
{
    public partial class WCFBasisService : ServiceBase
    {
        private readonly WcfServiceHost _wcfServiceHost;
        
        public WCFBasisService()
        {
            InitializeComponent();
            
            _wcfServiceHost = new WcfServiceHost(new ServiceApi(
                new ClientsRepository(), 
                new ServiceActionsFactory()
            ));
        }

        protected override void OnStart(string[] args)
        {
            _wcfServiceHost.Open();
        }

        protected override void OnStop()
        {
            _wcfServiceHost.Close();
        }
    }
}
