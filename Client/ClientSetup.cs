using System;
using System.Net.Security;
using System.ServiceModel;
using System.Timers;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Timer = System.Timers.Timer;

namespace Client
{
    /// <summary>
    /// Class setups the connection to the WCF service and expose the proxy object to call methods on service side
    /// </summary>
    public class ClientSetup
    {
        /// <summary>
        /// Communication channel factory
        /// </summary>
        private DuplexChannelFactory<IServiceApi> _channelFactory;

        /// <summary>
        /// Named pipes configuration
        /// </summary>
        private readonly NetNamedPipeBinding _netNamedPipeBinding;

        /// <summary>
        /// Communication proxy with service
        /// </summary>
        public IServiceApi ProxyChannel => _createProxyChannel();

        private readonly ICallbacksApi _callbackImplementation;
        
        public bool IsRegistered { get; protected set; }

        private ServiceStatus _lastServiceStatus = ServiceStatus.Functional;
        
        public Guid Id;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientSetup(ICallbacksApi callbackImplementation)
        {
            _callbackImplementation = callbackImplementation;
            
            Id = Guid.NewGuid();

            var updateChannelTimer = new Timer(1000);
            updateChannelTimer.Elapsed += _onUpdateChannelTimerElapsed;
            updateChannelTimer.Enabled = true;
            updateChannelTimer.AutoReset = true;
            updateChannelTimer.Start();
            
            _netNamedPipeBinding = new NetNamedPipeBinding
            {
                Security = { Mode = NetNamedPipeSecurityMode.Transport, Transport = new NamedPipeTransportSecurity() {ProtectionLevel = ProtectionLevel.EncryptAndSign} },
                MaxConnections = 10,
                OpenTimeout = new TimeSpan(0, 0, 30),
                ReceiveTimeout = new TimeSpan(0, 0, 5),
                SendTimeout = new TimeSpan(0, 0, 5),
                
            };
        }

        public void Register()
        {
            try
            {
                ProxyChannel.ActionRequest(new ActionModel
                    {ClientId = Id, Type = ActionType.RegisterClient, ExecuteImmediately = true});
                
                IsRegistered = true;

                _lastServiceStatus = ServiceStatus.Functional;
            }
            catch (EndpointNotFoundException){}
        }

        public void Unregister()
        {
            try
            {
                ProxyChannel.ActionRequest(new ActionModel
                    {ClientId = Id, Type = ActionType.UnregisterClient, ExecuteImmediately = true});
                
                IsRegistered = false;
            }
            catch (EndpointNotFoundException){}
        }

        private void _onUpdateChannelTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if(_lastServiceStatus == ServiceStatus.Functional)
                {
                    ProxyChannel.ActionRequest(new ActionModel
                        {ClientId = Id, Type = ActionType.UpdateChannel, ExecuteImmediately = true});
                }
                else
                {
                    this.Register();
                }
            }
            catch (EndpointNotFoundException)
            {
                IsRegistered = false;
                _lastServiceStatus = ServiceStatus.Faulted;
            }
        }
        

        /// <summary>
        /// Method creates new channel for the current request
        /// </summary>
        /// <returns>Return new communication channel</returns>
        private IServiceApi _createProxyChannel()
        {
            if(_channelFactory == null)
                _channelFactory = _createChannelFactory();

            if (_channelFactory.State == CommunicationState.Faulted)
            {
                _cleanChannelFactory();
                _channelFactory = _createChannelFactory();
            }

            return _channelFactory.CreateChannel();
        }

        /// <summary>
        /// Method aborts all operations on channel factory and then closing it
        /// </summary>
        private void _cleanChannelFactory()
        {
            if(_channelFactory == null) return;
            
            _channelFactory.Abort();
            _channelFactory.Close();
        }

        /// <summary>
        /// Method raised by event when channel factory get faulted state. In that case we have to re-create the channel factory.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void _onChannelFactoryFailure(object sender, EventArgs e)
        {
            _cleanChannelFactory();
            
            _channelFactory = _createChannelFactory();
        }

        /// <summary>
        /// Methods create the channel factory. Channel factory is used to create communication channels for each request.
        /// </summary>
        private DuplexChannelFactory<IServiceApi> _createChannelFactory()
        {
            var channelFactory = new DuplexChannelFactory<IServiceApi>(_callbackImplementation, _netNamedPipeBinding, new EndpointAddress("net.pipe://localhost/WCFBasis"));
            channelFactory.Faulted += _onChannelFactoryFailure;

            return channelFactory;
        }
    }
}