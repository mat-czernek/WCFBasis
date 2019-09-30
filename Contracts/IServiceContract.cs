using System.ServiceModel;
using Contracts.Models;

namespace Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IClientCallbackContract))]
    public interface IServiceContract
    {
        [OperationContract(IsOneWay = true)]
        void ActionRequest(ActionModel actionModel);
    }
}