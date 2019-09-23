using System.ServiceModel;
using Contracts.Models;

namespace Contracts
{
    /// <summary>
    /// Interface defines contract between WCF service and WCF client
    /// All methods may be called by client on service side
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallbacksApi))]
    public interface IServiceApi
    {
        [OperationContract(IsOneWay = true)]
        void ActionRequest(ActionModel actionModel);
    }
}