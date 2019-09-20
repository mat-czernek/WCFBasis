using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts.Delegates;
using Contracts.Enums;
using Contracts.Models;

namespace Contracts
{
    /// <summary>
    /// Interface defines contract between WCF service and WCF client
    /// All methods may be called by client on service side
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallbacksApi))]
    public interface IServiceOperationsApi
    {
        [OperationContract(IsOneWay = true)]
        void ActionRequest(ActionModel actionModel);
    }
}