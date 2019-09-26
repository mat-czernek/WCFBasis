using Contracts.Models;
using Service.Notifications;

namespace Service.Actions
{
    public interface IServiceActionsHandler
    {
        IServiceAction CreateActionFromModel(ActionModel actionModel);

        void PutActionInQueue(IServiceAction serviceAction);
    }
}