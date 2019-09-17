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
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceOperationsApi : IServiceOperationsApi
    {
        private static readonly object SyncObject = new object();
        
        /// <summary>
        /// Gets the list of registered client
        /// </summary>
        private readonly List<ClientModel> _registeredClients;

        private ProcessActions _processActions;

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
            _registeredClients = new List<ClientModel>();
        }


        private void _executeMethodOnCollection<T>(Action<T> action, List<T> collection)
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
            _processActions = new ProcessActions();
            
            _executeMethodOnCollection(
                client => client.CallbacksApiChannel.UpdateActionsQueue(_processActions.Actions
                    .FindAll(act => act.Status != ActionStatus.Completed).ToList()), _registeredClients);
            
            foreach (var action in _processActions.Actions)
            {
                _executeMethodOnCollection(client => client.CallbacksApiChannel.SetCurrentlyProcessedAction(action),
                    _registeredClients);
                
                
                Thread.Sleep(action.Delay);

                action.Status = ActionStatus.Completed;
                
                _executeMethodOnCollection(
                    client => client.CallbacksApiChannel.UpdateActionsQueue(_processActions.Actions
                        .FindAll(act => act.Status != ActionStatus.Completed).ToList()), _registeredClients);
            }

            _executeMethodOnCollection(client => client.CallbacksApiChannel.BroadcastMessage("All actions completed!"),
                _registeredClients);
        }
    }
}