using System;
using System.Net.Security;
using System.ServiceModel;
using Contracts;

namespace Service
{
    /// <summary>
    /// Class creates instance of the WCF service
    /// </summary>
    public class Host
    {
        private ServiceHost _serviceHost;
        
        private readonly NetNamedPipeBinding _netNamedPipeBinding;
        
        public bool IsOpened { get; protected set; } = false;
        
        private readonly IServiceApi _serviceApi;
        
        public Host(IServiceApi serviceApi)
        {
            _netNamedPipeBinding = new NetNamedPipeBinding
            {
                Security = { Mode = NetNamedPipeSecurityMode.Transport, Transport = new NamedPipeTransportSecurity() {ProtectionLevel = ProtectionLevel.EncryptAndSign} },
                MaxConnections = 10,
                OpenTimeout = new TimeSpan(0, 0, 30),
                ReceiveTimeout = new TimeSpan(0, 0, 10),
                SendTimeout = new TimeSpan(0, 0, 5)
            };

            _serviceApi = serviceApi;

            _serviceHost = _initalizeServiceHost();
        }


        private ServiceHost _initalizeServiceHost()
        {
            var serviceHost = new ServiceHost(_serviceApi, new Uri("net.pipe://localhost"));
            serviceHost.AddServiceEndpoint(typeof(IServiceApi), _netNamedPipeBinding, "WCFBasis");
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

            _serviceHost = _initalizeServiceHost();
        }
        
        private void _onHostOpened(object sender, EventArgs e)
        {
            IsOpened = true;
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