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
        /// <summary>
        /// Register new client in WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        [OperationContract]
        OperationReturnType RegisterClient(Guid id);

        /// <summary>
        /// Unregister client from WCF service
        /// </summary>
        /// <param name="id">Client unique Id</param>
        [OperationContract]
        OperationReturnType UnregisterClient(Guid id);

        [OperationContract(IsOneWay = true)]
        void UpdateChannel(Guid id);

        [OperationContract(IsOneWay = true)]
        void TakeActions();
    }
}