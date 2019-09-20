using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Contracts.Enums;
using Service.Services;

namespace Service.Actions
{
    public class SampleOperationAction : IAction
    {
        /// <summary>
        /// Thread synchronization objecy
        /// </summary>
        private static readonly object SyncObject = new object();
        
        public ActionType Type { get; set; } = ActionType.SampleOperation;
        
        public ActionStatus Status { get; set; } = ActionStatus.Idle;

        public Guid ClientId { get; }
        
        public SampleOperationAction(Guid clientId)
        {
            ClientId = clientId;
        }
        
        public void Take()
        {
            var processOperations = new ProcessOperations();
            
            _executeMethodOnCollectionItem(
                client => client.CallbacksApiChannel.UpdateActionsQueue(processOperations.Operations
                    .FindAll(act => act.Status != OperationStatus.Completed).ToList()), ServiceOperationsApi.RegisteredClients);

            foreach (var operation in processOperations.Operations)
            {
                _executeMethodOnCollectionItem(client => client.CallbacksApiChannel.SetCurrentlyProcessedAction(operation),
                    ServiceOperationsApi.RegisteredClients);
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;
                
                _executeMethodOnCollectionItem(
                    client => client.CallbacksApiChannel.UpdateActionsQueue(processOperations.Operations
                        .FindAll(act => act.Status != OperationStatus.Completed).ToList()), ServiceOperationsApi.RegisteredClients);
            }
            
            _executeMethodOnCollectionItem(client => client.CallbacksApiChannel.BroadcastMessage("All actions completed!"),
                ServiceOperationsApi.RegisteredClients);
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

            for(var i = 0; i < collection.Count; i++)
            {
                try
                {
                    action(collection[i]);
                }
                catch (CommunicationException) {}
            }
            
        }
    }
}