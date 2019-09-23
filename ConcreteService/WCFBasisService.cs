using System.ServiceProcess;
using Service;
using Service.Actions;
using Service.Services;

namespace ConcreteService
{
    public partial class WCFBasisService : ServiceBase
    {
        private readonly Host _host;
        
        public WCFBasisService()
        {
            InitializeComponent();
            
            _host = new Host(new ServiceApi(
                new ClientsRepository(), 
                new ServiceActionsFactory()
            ));
        }

        protected override void OnStart(string[] args)
        {
            _host.Open();
        }

        protected override void OnStop()
        {
            _host.Close();
        }
    }
}
