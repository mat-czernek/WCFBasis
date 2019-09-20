using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

namespace Service.Actions
{
    public class RegisterClientAction : IAction
    {
        private static readonly object SyncObject = new object();
        
        public Guid ClientId { get; }

        private readonly ICallbacksApi _operationContext; 
        
        public RegisterClientAction(Guid clientId, ICallbacksApi operationContext)
        { 
            ClientId = clientId;
            _operationContext = operationContext;
        }
        
        public ActionType Type { get; set; } = ActionType.RegisterClient;
        
        public ActionStatus Status { get; set; } = ActionStatus.Idle;
        
        public void Take()
        {
            if (ClientId == Guid.Empty) 
                Status = ActionStatus.Completed;

            lock (SyncObject)
            {
                if (ServiceOperationsApi.RegisteredClients.FindIndex(client => client.Id == ClientId) >= 0)
                    Status = ActionStatus.Completed;
            
                var clientModel = new ClientModel
                {
                    Id = ClientId,
                    CallbacksApiChannel = _operationContext,
                    RegistrationTime = DateTime.Now
                };
        
                ServiceOperationsApi.RegisteredClients.Add(clientModel);
                
                Status = ActionStatus.Completed;
                
                clientModel.CallbacksApiChannel.BroadcastMessage("Client registered successfully!");
            }
        }
    }
}