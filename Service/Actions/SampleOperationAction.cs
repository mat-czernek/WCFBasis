using System;
using System.Linq;
using System.Threading;
using Contracts.Enums;
using Service.Delegates;
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
        public ActionType Type { get; private set; } = ActionType.SampleOperation;
        
        public ActionStatus Status { get; private set; } = ActionStatus.Idle;
        
        public Guid ClientId { get; }

        public event OnOperationChangeDelegate OnOperationChange;

        public event OnOperationsListChangeDelegate OnOperationsListChange;

        public event OnOperationsCompletedDelegate OnOperationsCompleted;
        
        public SampleOperationAction(Guid clientId)
        {
            ClientId = clientId;
        }
        
       
        public void Take()
        {
            Status = ActionStatus.InProgress;
            
            var processOperations = new SampleOperations();

            OnOperationsListChange?.Invoke(processOperations.Operations);
            
            foreach (var operation in processOperations.Operations)
            {
                OnOperationChange?.Invoke(operation);
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;
                
                OnOperationsListChange?.Invoke(processOperations.Operations.FindAll(op => op.Status != OperationStatus.Completed).ToList());
            }
            
            OnOperationsCompleted?.Invoke();

            Status = ActionStatus.Completed;
        }
    }
}