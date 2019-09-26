using System.ServiceModel;
using Contracts;
using Contracts.Models;
using Service.Actions;
using Timer = System.Timers.Timer;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceContract : IServiceContract
    {
        private static readonly object ThreadSyncObject = new object();

        private readonly IServiceActionsHandler _serviceActionsHandler;
        
        public ServiceContract(IServiceActionsHandler serviceActionsHandler)
        {
            _serviceActionsHandler = serviceActionsHandler;
        }
        
        public void ActionRequest(ActionModel actionModel)
        {
            lock (ThreadSyncObject)
            {
                var requestedAction = _serviceActionsHandler.CreateActionFromModel(actionModel);
                
                if(requestedAction is InvalidAction) return;

                if (actionModel.ExecuteImmediately)
                {
                    requestedAction.Execute();
                }
                else
                {
                    _serviceActionsHandler.PutActionInQueue(requestedAction);
                }
            }
        }
    }
}