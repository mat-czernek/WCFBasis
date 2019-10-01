using System.Collections.Generic;
using System.ServiceModel;
using Contracts.Delegates;
using Contracts.Models;

namespace Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IClientCallbackContract
    {
        ServiceSimpleMessageDelegate GeneralStatusChanged { get; set; }
        
        ServiceActionsQueueDelegate OperationsQueueChanged { get; set; }
        
        ServiceCurrentActionDelegate CurrentActionChanged { get; set; }
        
        [OperationContract(IsOneWay = true)]
        void UpdateGeneralStatus(string text);

        [OperationContract(IsOneWay = true)]
        void UpdateCurrentOperation(SampleOperationModel sampleOperation);

        [OperationContract(IsOneWay = true)]
        void UpdateOperationsQueue(List<SampleOperationModel> actions);
    }
}