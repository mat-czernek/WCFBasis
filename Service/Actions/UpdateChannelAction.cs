using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;

namespace Service.Actions
{
    public class UpdateChannelAction : IAction
    {
        private static readonly object SyncObject = new object();
        
        public ActionType Type { get; set; } = ActionType.UpdateChannel;

        public ActionStatus Status { get; set; } = ActionStatus.Idle;
        
        public Guid ClientId { get; }

        private readonly ICallbacksApi _operationContext; 

        public UpdateChannelAction(Guid clientId, ICallbacksApi operationContext)
        {
            ClientId = clientId;
            _operationContext = operationContext;
        }
        
        public void Take()
        {

            var clientIndex = ServiceOperationsApi.RegisteredClients.FindIndex(client => client.Id == ClientId);

            if (clientIndex >= 0)
            {
                ServiceOperationsApi.RegisteredClients[clientIndex].CallbacksApiChannel = _operationContext;
            }

            Status = ActionStatus.Completed;
            
      
        }
    }
}