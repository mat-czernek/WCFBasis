using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Contracts.Delegates;
using Contracts.Models;

namespace Contracts
{
    /// <summary>
    /// Interface defines contract between client and service
    /// All methods may be call by service on client side
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICallbacks
    {
        ServiceSimpleMessageDelegate ServiceSimpleMessage { get; set; }
        
        ServiceActionsQueueDelegate ServiceActionsQueue { get; set; }
        
        ServiceCurrentActionDelegate ServiceCurrentAction { get; set; }

        /// <summary>
        /// Sample method executed by service on client side
        /// </summary>
        /// <param name="text"></param>
        [OperationContract(IsOneWay = true)]
        void BroadcastMessage(string text);

        [OperationContract(IsOneWay = true)]
        void SetActionsInQueue(List<ActionModel> actions);

        [OperationContract(IsOneWay = true)]
        void SetCurrentlyProcessedAction(ActionModel action);
    }
}