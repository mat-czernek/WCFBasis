using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

namespace Service.Actions
{
    public class UnregisterClientAction : IAction
    {
        private static readonly object SyncObject = new object();
        
        private readonly Guid _clientId;
        
        public UnregisterClientAction(Guid clientId)
        {
            _clientId = clientId;
        }

        public ActionType Type { get; set; } = ActionType.RegisterClient;

        public ActionStatus Status { get; set; } = ActionStatus.Idle;

        public Guid ClientId => _clientId;

        public void Take()
        {
            if(_clientId == Guid.Empty) Status = ActionStatus.Completed;


                var clientToUnregister = ServiceOperationsApi.RegisteredClients.SingleOrDefault(client => client.Id == _clientId);

                if (clientToUnregister == null)
                {
                    Status = ActionStatus.Completed;
                    return;
                }
                
                ServiceOperationsApi.RegisteredClients.Remove(clientToUnregister);
                
                Status = ActionStatus.Completed;
            
        }
    }
}