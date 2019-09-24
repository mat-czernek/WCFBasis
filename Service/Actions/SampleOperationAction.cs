using System;
using System.Linq;
using System.Threading;
using Contracts.Enums;
using Service.Notifications;
using Service.Services;

namespace Service.Actions
{
    /// <summary>
    /// The class implements operation on sample class that imitates time consuming operations
    /// This has been done to show that this type of operation does not block the other methods executed by client (like update channel, register or unregister)
    /// </summary>
    public class SampleOperationAction : IServiceAction
    {
        public ActionType Type { get; private set; } = ActionType.SampleOperation;
        
        public ActionStatus Status { get; private set; } = ActionStatus.Idle;
        
        public Guid ClientId { get; }

        public SampleOperationAction(Guid clientId)
        {
            ClientId = clientId;
        }
        
       
        public void Take()
        {
            Status = ActionStatus.InProgress;
            
            var processOperations = new SampleOperations();

            ClientNotificationFactory.OperationsQueueUpdate(processOperations.Operations).NotifyAll();
            
            foreach (var operation in processOperations.Operations)
            {
                ClientNotificationFactory.CurrentOperationUpdate(operation).NotifyAll();
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;
                
                ClientNotificationFactory.OperationsQueueUpdate(processOperations.Operations
                    .FindAll(op => op.Status != OperationStatus.Completed).ToList()).NotifyAll();
            }
            
            ClientNotificationFactory.GeneralNotification("All operations have been completed successfully!").NotifyAll();

            Status = ActionStatus.Completed;
        }
    }
}