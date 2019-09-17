using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Contracts;
using Contracts.Delegates;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;

namespace Service
{
    /// <summary>
    /// Class implements operations performed by WCF service
    /// All methods may be called by WCF client
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceOperationsApi : IServiceOperationsApi
    {
        /// <summary>
        /// Thread synchronization objecy
        /// </summary>
        private static readonly object SyncObject = new object();
        
        /// <summary>
        /// The list of registered clients
        /// </summary>
        private readonly List<ClientModel> _registeredClients;

        /// <summary>
        /// Sample class with action to process
        /// </summary>
        private ProcessActions _processActions;

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static ServiceOperationsApi _instance = null;

        /// <summary>
        /// Creates the singleton instance of the class
        /// </summary>
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
            _registeredClients = new List<ClientModel>();
        }


        /// <summary>
        /// Method enumerates items on the list and executes item method degined in parameter.
        /// This method it's used to avoid multiple foreach statement in cases when collection needs to be enumerated
        /// </summary>
        /// <param name="action">Method to be called by collection item</param>
        /// <param name="collection">Target collection</param>
        /// <typeparam name="T">The type of the collection item</typeparam>
        private void _executeMethodOnCollectionItem<T>(Action<T> action, List<T> collection)
        {
            var inactiveClients = new List<T>();

            lock (SyncObject)
            {

                foreach (var item in collection)
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
            
                collection = collection.Except(inactiveClients).ToList();
            }
        }

        /// <summary>
        /// Method registers client in WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        public OperationReturnType RegisterClient(Guid id)
        {
            if(id == Guid.Empty) return OperationReturnType.Failure;

            lock (SyncObject)
            {
                if (_registeredClients.FindIndex(client => client.Id == id) >= 0) return OperationReturnType.ClientAlreadyExist;
            
                var clientModel = new ClientModel
                {
                    Id = id,
                    CallbacksApiChannel = OperationContext.Current.GetCallbackChannel<ICallbacksApi>(),
                    RegistrationTime = DateTime.Now
                };
        
                _registeredClients.Add(clientModel);
            }

            return OperationReturnType.Success;
        }
        
        /// <summary>
        /// Method un-registers client from the WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        public OperationReturnType UnregisterClient(Guid id)
        {
            if(id == Guid.Empty) return OperationReturnType.Failure;

            lock (SyncObject)
            {
                var clientToUnregister = _registeredClients.SingleOrDefault(client => client.Id == id);

                if(clientToUnregister == null) return OperationReturnType.ClientNotExist;

                _registeredClients.Remove(clientToUnregister);
            }

            return OperationReturnType.Success;
        }

        /// <summary>
        /// Method updates client communication channel
        /// </summary>
        /// <param name="id"></param>
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
        
        
        /// <summary>
        /// Method executes sample actions and send status to the registered clients
        /// </summary>
        public void TakeActions()
        {
            _processActions = new ProcessActions();
            
            _executeMethodOnCollectionItem(
                client => client.CallbacksApiChannel.UpdateActionsQueue(_processActions.Actions
                    .FindAll(act => act.Status != ActionStatus.Completed).ToList()), _registeredClients);
            
            foreach (var action in _processActions.Actions)
            {
                _executeMethodOnCollectionItem(client => client.CallbacksApiChannel.SetCurrentlyProcessedAction(action),
                    _registeredClients);
                
                
                Thread.Sleep(action.Delay);

                action.Status = ActionStatus.Completed;
                
                _executeMethodOnCollectionItem(
                    client => client.CallbacksApiChannel.UpdateActionsQueue(_processActions.Actions
                        .FindAll(act => act.Status != ActionStatus.Completed).ToList()), _registeredClients);
            }

            _executeMethodOnCollectionItem(client => client.CallbacksApiChannel.BroadcastMessage("All actions completed!"),
                _registeredClients);
        }
    }
}