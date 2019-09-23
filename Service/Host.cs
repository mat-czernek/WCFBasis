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
        /// <summary>
        /// WCF service instance
        /// </summary>
        private ServiceHost _serviceHost;

        /// <summary>
        /// Named pipes configuration
        /// </summary>
        private readonly NetNamedPipeBinding _netNamedPipeBinding;

        /// <summary>
        /// Indicates whether the service host has been opened successfully or not
        /// </summary>
        public bool IsOpened { get; protected set; } = false;

        
        private readonly IServiceApi _serviceApi;

        /// <summary>
        /// Default constructor. Initiates the named pipes configuration and service host instance
        /// </summary>
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

        /// <summary>
        /// Method initialize service host instance
        /// </summary>
        /// <returns>Returns the service host instance</returns>
        private ServiceHost _initalizeServiceHost()
        {
            var serviceHost = new ServiceHost(_serviceApi, new Uri("net.pipe://localhost"));
            serviceHost.AddServiceEndpoint(typeof(IServiceApi), _netNamedPipeBinding, "WCFBasis");
            serviceHost.Faulted += _onHostFailure;
            serviceHost.Opened += _onHostOpened;
            
            return serviceHost;
        }

        /// <summary>
        /// Method close the service host
        /// </summary>
        private void _closeServiceHost()
        {
            if (_serviceHost == null) return;
            
            _serviceHost.Abort();
            _serviceHost.Close();
            _serviceHost = null;
        }

        /// <summary>
        /// Method executed on host failure event. Method performs actions to completely close the service host and open re-initalize it again
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void _onHostFailure(object sender, EventArgs e)
        {
            _closeServiceHost();

            _serviceHost = _initalizeServiceHost();
        }

        /// <summary>
        /// Method executed when service host has been opened successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _onHostOpened(object sender, EventArgs e)
        {
            IsOpened = true;
        }

        /// <summary>
        /// Method opens the service host
        /// </summary>
        public void Open()
        {
            _serviceHost.Open();
        }

        /// <summary>
        /// Method closes the service host
        /// </summary>
        public void Close()
        {
            _closeServiceHost();
        }
    }
}