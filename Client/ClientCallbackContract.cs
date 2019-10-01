using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Delegates;
using Contracts.Models;

namespace Client
{
    public class ClientCallbackContract : IClientCallbackContract
    {
        public ServiceSimpleMessageDelegate GeneralStatusChanged { get; set; }
        
        public ServiceActionsQueueDelegate OperationsQueueChanged { get; set; }
        
        public ServiceCurrentActionDelegate CurrentActionChanged { get; set; }


        public void UpdateGeneralStatus(string text)
        {
            GeneralStatusChanged?.Invoke(text);
        }

        public void UpdateCurrentOperation(SampleOperationModel sampleOperation)
        {
            CurrentActionChanged?.Invoke(sampleOperation);
        }

        public void UpdateOperationsQueue(List<SampleOperationModel> actions)
        {
            OperationsQueueChanged?.Invoke(actions);
        }
    }
}