using System;
using System.Net.Security;
using System.ServiceModel;
using Contracts;

namespace Service
{
    public class WcfServiceHost
    {
        private ServiceHost _serviceHost;
        
        private readonly NetNamedPipeBinding _servicePipeBinding;

        private readonly IServiceContract _serviceContract;
        
        public WcfServiceHost(IServiceContract serviceContract)
        {
            _servicePipeBinding = new NetNamedPipeBinding
            {
                Security =
                {
                    Mode = NetNamedPipeSecurityMode.Transport,
                    Transport = new NamedPipeTransportSecurity() {ProtectionLevel = ProtectionLevel.EncryptAndSign}
                },
                MaxConnections = 10,
                OpenTimeout = new TimeSpan(0, 0, 30),
                ReceiveTimeout = new TimeSpan(0, 0, 10),
                SendTimeout = new TimeSpan(0, 0, 5)
            };

            _serviceContract = serviceContract;

            _serviceHost = _createServiceHost();
        }


        private ServiceHost _createServiceHost()
        {
            var serviceHost = new ServiceHost(_serviceContract, new Uri("net.pipe://localhost"));
            serviceHost.AddServiceEndpoint(typeof(IServiceContract), _servicePipeBinding, "WCFBasis");
            serviceHost.Faulted += _onHostFailure;
            serviceHost.Opened += _onHostOpened;
            
            return serviceHost;
        }
        
        private void _closeServiceHost()
        {
            if (_serviceHost == null) return;
            
            _serviceHost.Abort();
            _serviceHost.Close();
            _serviceHost = null;
        }
        
        private void _onHostFailure(object sender, EventArgs e)
        {
            _closeServiceHost();

            _serviceHost = _createServiceHost();
        }
        
        private void _onHostOpened(object sender, EventArgs e)
        {
            
        }
        
        public void Open()
        {
            _serviceHost.Open();
        }
        
        public void Close()
        {
            _closeServiceHost();
        }
    }
}