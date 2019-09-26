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
        private DuplexChannelFactory<IServiceContract> _duplexChannelFactory;
        
        private readonly NetNamedPipeBinding _clientPipeBinding;

        public IServiceContract ServiceCommunicationChannel => _createCommunicationChannel();

        private readonly IClientCallbackContract _clientCallbackContractImplementation;
        
        public bool IsRegistered { get; protected set; }

        private ServiceStatus _lastServiceStatus = ServiceStatus.Functional;
        
        public Guid ClientId;
        
        public ClientSetup(IClientCallbackContract clientCallbackContractImplementation)
        {
            _clientCallbackContractImplementation = clientCallbackContractImplementation;
            
            ClientId = Guid.NewGuid();

            var updateCommunicationChannelTimer = new Timer(1000);
            updateCommunicationChannelTimer.Elapsed += _updateCommunicationChannel;
            updateCommunicationChannelTimer.Enabled = true;
            updateCommunicationChannelTimer.AutoReset = true;
            updateCommunicationChannelTimer.Start();
            
            _clientPipeBinding = new NetNamedPipeBinding
            {
                Security =
                {
                    Mode = NetNamedPipeSecurityMode.Transport,
                    Transport = new NamedPipeTransportSecurity() {ProtectionLevel = ProtectionLevel.EncryptAndSign}
                },
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
                ServiceCommunicationChannel.ActionRequest(new ActionModel
                    { ClientId = ClientId, Type = ActionType.RegisterClient, ExecuteImmediately = true });
                
                IsRegistered = true;

                _lastServiceStatus = ServiceStatus.Functional;
            }
            catch (EndpointNotFoundException){IsRegistered = false;}
        }

        public void Unregister()
        {
            try
            {
                ServiceCommunicationChannel.ActionRequest(new ActionModel
                    { ClientId = ClientId, Type = ActionType.UnregisterClient, ExecuteImmediately = true });
                
                IsRegistered = false;
            }
            catch (EndpointNotFoundException){IsRegistered = false;}
        }

        private void _updateCommunicationChannel(object sender, ElapsedEventArgs e)
        {
            try
            {
                if(_lastServiceStatus == ServiceStatus.Functional)
                {
                    ServiceCommunicationChannel.ActionRequest(new ActionModel
                        { ClientId = ClientId, Type = ActionType.UpdateChannel, ExecuteImmediately = true });
                }
                else
                {
                    Register();
                }
            }
            catch (EndpointNotFoundException)
            {
                IsRegistered = false;
                _lastServiceStatus = ServiceStatus.Faulted;
            }
        }
        
        
        private IServiceContract _createCommunicationChannel()
        {
            if(_duplexChannelFactory == null)
                _duplexChannelFactory = _createDuplexChannelFactory();

            if (_duplexChannelFactory.State == CommunicationState.Faulted)
            {
                _resetDuplexCommunicationChannel();
                _duplexChannelFactory = _createDuplexChannelFactory();
            }

            return _duplexChannelFactory.CreateChannel();
        }
        
        private void _resetDuplexCommunicationChannel()
        {
            if(_duplexChannelFactory == null) return;
            
            _duplexChannelFactory.Abort();
            _duplexChannelFactory.Close();
        }
        
        private void _onChannelFactoryFailure(object sender, EventArgs e)
        {
            _resetDuplexCommunicationChannel();
            
            _duplexChannelFactory = _createDuplexChannelFactory();
        }
        
        private DuplexChannelFactory<IServiceContract> _createDuplexChannelFactory()
        {
            var channelFactory = new DuplexChannelFactory<IServiceContract>(_clientCallbackContractImplementation, _clientPipeBinding, new EndpointAddress("net.pipe://localhost/WCFBasis"));
            channelFactory.Faulted += _onChannelFactoryFailure;

            return channelFactory;
        }
    }
}