using System.Collections.Generic;
using System.ServiceModel;
using Contracts.Delegates;
using Contracts.Models;

namespace Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICallbackContract
    {
        GeneralStatusChangedDelegate GeneralStatusChanged { get; set; }
        
        OperationsQueueChangedDelegate OperationsQueueChanged { get; set; }
        
        CurrentOperationChangedDelegate CurrentOperationChanged { get; set; }
        
        [OperationContract(IsOneWay = true)]
        void NotifyGeneralStatusChanged(string text);

        [OperationContract(IsOneWay = true)]
        void NotifyCurrentOperationChanged(SampleOperationModel sampleOperation);

        [OperationContract(IsOneWay = true)]
        void NotifyOperationQueueChanged(List<SampleOperationModel> actions);
    }
}