using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace ConcreteService
{
    public partial class WCFBasisService : ServiceBase
    {
        private readonly Host _host;
        
        public WCFBasisService()
        {
            InitializeComponent();
            
            _host = new Host();
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
