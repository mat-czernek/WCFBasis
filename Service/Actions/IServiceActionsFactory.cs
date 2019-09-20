using Contracts.Models;

namespace Service.Actions
{
    public interface IServiceActionsFactory
    {
        IServiceAction Create(ActionModel model);
    }
}