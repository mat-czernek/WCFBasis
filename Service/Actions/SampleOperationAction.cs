using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Contracts.Enums;
using Service.Services;
using Service.Utilities;

namespace Service.Actions
{
    /// <summary>
    /// The class implements operation on sample class that imitates time consuming operations
    /// This has been done to show that this type of operation does not block the other methods executed by client (like update channel, register or unregister)
    /// </summary>
    public class SampleOperationAction : IServiceAction
    {
        /// <summary>
        /// Thread synchronization context
        /// </summary>
        private static readonly object SyncObject = new object();
        
        /// <summary>
        /// Gets the action type
        /// </summary>
        public ActionType Type { get; set; } = ActionType.SampleOperation;
        
        /// <summary>
        /// Gets the action status
        /// </summary>
        public ActionStatus Status { get; set; } = ActionStatus.Idle;

        /// <summary>
        /// WCF client Id
        /// </summary>
        public Guid ClientId { get; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="clientId"></param>
        public SampleOperationAction(Guid clientId)
        {
            ClientId = clientId;
        }
        
        /// <summary>
        /// Method executes operations defined in ProcessOperations class
        /// This has been done to simulate the time consuming operations and show how to send the status for each of them to the client
        /// </summary>
        public void Take()
        {
            var processOperations = new ProcessOperations();

            CollectionsOperations.ExecuteMethodOnItems(
                client => client.CallbacksApiChannel.UpdateActionsQueue(processOperations.Operations
                    .FindAll(act => act.Status != OperationStatus.Completed).ToList()), ServiceOperationsApi.RegisteredClients);

            Status = ActionStatus.InProgress;
            
            foreach (var operation in processOperations.Operations)
            {
                CollectionsOperations.ExecuteMethodOnItems(client => client.CallbacksApiChannel.SetCurrentlyProcessedAction(operation),
                    ServiceOperationsApi.RegisteredClients);
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;

                CollectionsOperations.ExecuteMethodOnItems(
                    client => client.CallbacksApiChannel.UpdateActionsQueue(processOperations.Operations
                        .FindAll(act => act.Status != OperationStatus.Completed).ToList()), ServiceOperationsApi.RegisteredClients);
            }

            Status = ActionStatus.Completed;

            CollectionsOperations.ExecuteMethodOnItems(client => client.CallbacksApiChannel.Message("All actions completed!"),
                ServiceOperationsApi.RegisteredClients);
        }
    }
}