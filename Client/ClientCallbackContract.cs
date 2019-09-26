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
    //[CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallbackContract : IClientCallbackContract
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

        public void UpdateCurrentOperation(SampleOperationModel sampleOperation)
        {
            ServiceCurrentAction?.Invoke(sampleOperation);
        }

        public void UpdateOperationsQueue(List<SampleOperationModel> actions)
        {
            ServiceActionsQueue?.Invoke(actions);
        }
    }
}