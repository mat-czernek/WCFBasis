using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Delegates;
using Contracts.Models;

namespace Client
{
    /// <summary>
    /// Class implements method called by WCF service on client side
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CallbacksApi : ICallbacksApi
    {
        public ServiceSimpleMessageDelegate ServiceSimpleMessage { get; set; }
        
        public ServiceActionsQueueDelegate ServiceActionsQueue { get; set; }
        
        public ServiceCurrentActionDelegate ServiceCurrentAction { get; set; }

        /// <summary>
        /// Method displays message from the service
        /// </summary>
        /// <param name="text">String message to be sent by service to client</param>
        public void UpdateGeneralStatus(string text)
        {
            ServiceSimpleMessage?.Invoke(text);
        }

        public void UpdateCurrentOperation(OperationModel operation)
        {
            ServiceCurrentAction?.Invoke(operation);
        }

        public void UpdateOperationsQueue(List<OperationModel> actions)
        {
            ServiceActionsQueue?.Invoke(actions);
        }
    }
}