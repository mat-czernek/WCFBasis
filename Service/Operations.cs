using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Contracts;
using Contracts.Models;
using Service.Delegates;
using Service.Services;

namespace Service
{
    /// <summary>
    /// Class implements operations performed by WCF service
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    [ServiceKnownType(typeof(List<ClientModel>))]
    public class Operations : IOperations
    {
        private static object _syncObject = new object();
        
        /// <summary>
        /// Gets the list of registered client
        /// </summary>
        private List<ClientModel> _registeredClients;

        private ProcessActions _processActions;

        public event EmitSimpleMessageDelegate OnClientRegistration;

        public event EmitSimpleMessageDelegate OnClientUnregistration;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Operations()
        {
            _processActions = new ProcessActions();
            
            _registeredClients = new List<ClientModel>();
        }
        
        /// <summary>
        /// Method registers client in WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        public void RegisterClient(Guid id)
        {
            if(id == Guid.Empty) return;

            if (_registeredClients.FindIndex(client => client.Id == id) >= 0) return;
            
            var clientModel = new ClientModel
            {
                Id = id,
                CallbacksChannel = OperationContext.Current.GetCallbackChannel<ICallbacks>(),
                RegistrationTime = DateTime.Now
            };
        
            _registeredClients.Add(clientModel);

            OnClientRegistration?.Invoke(clientModel.Id.ToString());
        }
        
        /// <summary>
        /// Method un-registers client from the WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        public void UnregisterClient(Guid id)
        {
            if(id == Guid.Empty) return;

            lock (_syncObject)
            {
                var clientIndex = _registeredClients.FindIndex(client => client.Id == id);

                if (clientIndex < 0) return;
            
                _registeredClients.RemoveAt(clientIndex);
                
                OnClientUnregistration?.Invoke(id.ToString());
            }
        }

        public void UpdateChannel(Guid id)
        {
            lock (_syncObject)
            {
                var clientIndex = _registeredClients.FindIndex(client => client.Id == id);

                if (clientIndex >= 0)
                {
                    _registeredClients[clientIndex].CallbacksChannel =
                        OperationContext.Current.GetCallbackChannel<ICallbacks>();
                }
            }
            
        }

        public void SendBroadcastMessage(string message)
        {
            lock (_syncObject)
            {
                var inactiveClients = new List<ClientModel>();
            
                foreach (var client in _registeredClients)
                {
                    try
                    {
                        client.CallbacksChannel.BroadcastMessage($"{client.Id} | Message: {message}");
                    }
                    catch (CommunicationException)
                    {
                        inactiveClients.Add(client);
                    }
                }

                _registeredClients = _registeredClients.Except(inactiveClients).ToList();
            }
        }

        public void SendToSelectedClients(string message, List<string> clientsIdList)
        {
            foreach (var clientId in clientsIdList)
            {
                var targetClient = _registeredClients.SingleOrDefault(client => client.Id == Guid.Parse(clientId));

                targetClient?.CallbacksChannel.BroadcastMessage(message);
            }
        }

        public void TakeActions()
        {
            foreach (var action in _processActions.Actions)
            {
                foreach (var client in _registeredClients)
                {
                    try
                    {
                        client.CallbacksChannel.SetCurrentlyProcessedAction(action);
                    }
                    catch (CommunicationException){}
                }
                
                Thread.Sleep(action.Delay);
            }

            foreach (var client in _registeredClients)
            {
                try
                {
                    client.CallbacksChannel.BroadcastMessage("All actions completed!");
                }
                catch (CommunicationException){}
            }
        }

        public void GetActions()
        {
            foreach (var client in _registeredClients)
            {
                try
                {
                    client.CallbacksChannel.SetActionsInQueue(_processActions.Actions);
                }
                catch (CommunicationException){}
            }
        }
    }
}