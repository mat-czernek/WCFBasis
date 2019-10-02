using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Delegates;
using Contracts.Models;

namespace Client
{
    public class CallbackContract : ICallbackContract
    {
        public GeneralStatusChangedDelegate GeneralStatusChanged { get; set; }
        
        public OperationsQueueChangedDelegate OperationsQueueChanged { get; set; }
        
        public CurrentOperationChangedDelegate CurrentOperationChanged { get; set; }


        public void NotifyGeneralStatusChanged(string text)
        {
            GeneralStatusChanged?.Invoke(text);
        }

        public void NotifyCurrentOperationChanged(SampleOperationModel sampleOperation)
        {
            CurrentOperationChanged?.Invoke(sampleOperation);
        }

        public void NotifyOperationQueueChanged(List<SampleOperationModel> actions)
        {
            OperationsQueueChanged?.Invoke(actions);
        }
    }
}