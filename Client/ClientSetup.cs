using System;
using System.ServiceModel;
using System.Timers;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

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
        private DuplexChannelFactory<IServiceOperationsApi> _channelFactory;

        /// <summary>
        /// Named pipes configuration
        /// </summary>
        private readonly NetNamedPipeBinding _netNamedPipeBinding;

        private readonly Timer _updateChannelTimer;
       
        /// <summary>
        /// Communication proxy with service
        /// </summary>
        public IServiceOperationsApi ProxyChannel => _createProxyChannel();

        private ICallbacksApi _callbackImplementation;
        
        public Guid Id;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientSetup(ICallbacksApi callbackImplementation)
        {
            _callbackImplementation = callbackImplementation;
            
            Id = Guid.NewGuid();

            _updateChannelTimer = new Timer(1000);
            _updateChannelTimer.Elapsed += _onUpdateChannelTimerElapsed;
            _updateChannelTimer.Enabled = true;
            _updateChannelTimer.AutoReset = true;
            _updateChannelTimer.Start();
            
            _netNamedPipeBinding = new NetNamedPipeBinding
            {
                Security = { Mode = NetNamedPipeSecurityMode.Transport },
                MaxConnections = 10,
                OpenTimeout = new TimeSpan(0, 0, 30),
                ReceiveTimeout = new TimeSpan(0, 0, 5),
                SendTimeout = new TimeSpan(0, 0, 5)
            };
        }

        private void _onUpdateChannelTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ProxyChannel.ActionRequest(new ActionModel {ClientId = Id, Type = ActionType.UpdateChannel, ExecuteImmediately = true});
        }

        /// <summary>
        /// Method creates new channel for the current request
        /// </summary>
        /// <returns>Return new communication channel</returns>
        private IServiceOperationsApi _createProxyChannel()
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
        private DuplexChannelFactory<IServiceOperationsApi> _createChannelFactory()
        {
            var channelFactory = new DuplexChannelFactory<IServiceOperationsApi>(_callbackImplementation, _netNamedPipeBinding, new EndpointAddress("net.pipe://localhost/WCFBasis"));
            channelFactory.Faulted += _onChannelFactoryFailure;

            return channelFactory;
        }
    }
}