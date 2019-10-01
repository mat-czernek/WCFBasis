using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Delegates;
using Contracts.Models;

namespace Client
{
    public class ClientCallbackContract : IClientCallbackContract
    {
        public ServiceSimpleMessageDelegate ServiceSimpleMessage { get; set; }
        
        public ServiceActionsQueueDelegate ServiceActionsQueue { get; set; }
        
        public ServiceCurrentActionDelegate ServiceCurrentAction { get; set; }


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