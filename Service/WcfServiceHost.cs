using System;
using System.Net.Security;
using System.ServiceModel;
using Contracts;

namespace Service
{
    public class WcfServiceHost
    {
        private ServiceHost _serviceHostInstance;
        
        private readonly NetNamedPipeBinding _serviceHostBinding;

        private readonly IServiceApi _serviceApi;
        
        public WcfServiceHost(IServiceApi serviceApi)
        {
            _serviceHostBinding = new NetNamedPipeBinding
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

            _serviceApi = serviceApi;

            _serviceHostInstance = _initalizeServiceHost();
        }


        private ServiceHost _initalizeServiceHost()
        {
            var serviceHost = new ServiceHost(_serviceApi, new Uri("net.pipe://localhost"));
            serviceHost.AddServiceEndpoint(typeof(IServiceApi), _serviceHostBinding, "WCFBasis");
            serviceHost.Faulted += _onHostFailure;
            serviceHost.Opened += _onHostOpened;
            
            return serviceHost;
        }
        
        private void _closeServiceHost()
        {
            if (_serviceHostInstance == null) return;
            
            _serviceHostInstance.Abort();
            _serviceHostInstance.Close();
            _serviceHostInstance = null;
        }
        
        private void _onHostFailure(object sender, EventArgs e)
        {
            _closeServiceHost();

            _serviceHostInstance = _initalizeServiceHost();
        }
        
        private void _onHostOpened(object sender, EventArgs e)
        {
            
        }
        
        public void Open()
        {
            _serviceHostInstance.Open();
        }
        
        public void Close()
        {
            _closeServiceHost();
        }
    }
}