using System.Collections.Generic;
using System.ServiceModel;
using Contracts.Delegates;
using Contracts.Models;

namespace Contracts
{
    /// <summary>
    /// Interface defines methods implemented on WCF client side that could be executed by WCF service
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IClientCallbackContract
    {
        ServiceSimpleMessageDelegate ServiceSimpleMessage { get; set; }
        
        ServiceActionsQueueDelegate ServiceActionsQueue { get; set; }
        
        ServiceCurrentActionDelegate ServiceCurrentAction { get; set; }

        /// <summary>
        /// Method sets the
        /// </summary>
        /// <param name="text"></param>
        [OperationContract(IsOneWay = true)]
        void UpdateGeneralStatus(string text);

        [OperationContract(IsOneWay = true)]
        void UpdateCurrentOperation(SampleOperationModel sampleOperation);

        [OperationContract(IsOneWay = true)]
        void UpdateOperationsQueue(List<SampleOperationModel> actions);
    }
}