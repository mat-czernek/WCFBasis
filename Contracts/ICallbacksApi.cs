using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Contracts.Delegates;
using Contracts.Models;

namespace Contracts
{
    /// <summary>
    /// Interface defines methods implemented on WCF client side that might be executed by WCF service
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICallbacksApi
    {
        ServiceSimpleMessageDelegate ServiceSimpleMessage { get; set; }
        
        ServiceActionsQueueDelegate ServiceActionsQueue { get; set; }
        
        ServiceCurrentActionDelegate ServiceCurrentAction { get; set; }

        /// <summary>
        /// Method sets the
        /// </summary>
        /// <param name="text"></param>
        [OperationContract(IsOneWay = true)]
        void BroadcastMessage(string text);

        [OperationContract(IsOneWay = true)]
        void SetCurrentlyProcessedAction(OperationModel operation);

        [OperationContract(IsOneWay = true)]
        void UpdateActionsQueue(List<OperationModel> actions);
    }
}