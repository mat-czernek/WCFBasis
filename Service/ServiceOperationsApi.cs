using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Contracts;
using Contracts.Delegates;
using Contracts.Models;
using Service.Services;

namespace Service
{
    /// <summary>
    /// Class implements operations performed by WCF service
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceOperationsApi : IServiceOperationsApi
    {
        private static readonly object SyncObject = new object();
        
        /// <summary>
        /// Gets the list of registered client
        /// </summary>
        private List<ClientModel> _registeredClients;

        private readonly ProcessActions _processActions;

        private static ServiceOperationsApi _instance = null;

        public static ServiceOperationsApi Instance
        {
            get
            {
                lock (SyncObject)
                {
                    return _instance ?? (_instance = new ServiceOperationsApi());
                }
            }
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        private ServiceOperationsApi()
        {
            _processActions = new ProcessActions();
            
            _registeredClients = new List<ClientModel>();
        }


        private void _executeMethodOnCollection<T>(Action<T> action, IEnumerable<T> collection)
        {
            var inactiveClients = new List<T>();

            lock (SyncObject)
            {
                var enumerable = collection.ToList();
                
                foreach (var item in enumerable)
                {
                    try
                    {
                        action(item);
                    }
                    catch (CommunicationException)
                    {
                        inactiveClients.Add(item);
                    }
                }
            
                collection = enumerable.Except(inactiveClients).ToList();
            }
        }

        /// <summary>
        /// Method registers client in WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        public void RegisterClient(Guid id)
        {
            if(id == Guid.Empty) return;

            lock (SyncObject)
            {
                if (_registeredClients.FindIndex(client => client.Id == id) >= 0) return;
            
                var clientModel = new ClientModel
                {
                    Id = id,
                    CallbacksApiChannel = OperationContext.Current.GetCallbackChannel<ICallbacksApi>(),
                    RegistrationTime = DateTime.Now
                };
        
                _registeredClients.Add(clientModel);
            }
            
        }
        
        /// <summary>
        /// Method un-registers client from the WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        public void UnregisterClient(Guid id)
        {
            if(id == Guid.Empty) return;

            lock (SyncObject)
            {
                var clientToUnregister = _registeredClients.SingleOrDefault(client => client.Id == id);

                if(clientToUnregister == null) return;

                _registeredClients.Remove(clientToUnregister);
            }
        }

        public void UpdateChannel(Guid id)
        {
            lock (SyncObject)
            {
                var clientIndex = _registeredClients.FindIndex(client => client.Id == id);

                if (clientIndex >= 0)
                {
                    _registeredClients[clientIndex].CallbacksApiChannel =
                        OperationContext.Current.GetCallbackChannel<ICallbacksApi>();
                }
            }
            
        }
        
        
        public void TakeActions()
        {
            foreach (var action in _processActions.Actions)
            {
                _executeMethodOnCollection(client => client.CallbacksApiChannel.SetCurrentlyProcessedAction(action),
                    _registeredClients);
                
                Thread.Sleep(action.Delay);
            }

            _executeMethodOnCollection(client => client.CallbacksApiChannel.BroadcastMessage("All actions completed!"),
                _registeredClients);
        }

        public void GetActions()
        {
            _executeMethodOnCollection(client => client.CallbacksApiChannel.SetActionsInQueue(_processActions.Actions),
                _registeredClients);
        }
    }
}