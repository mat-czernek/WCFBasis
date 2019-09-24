using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Models;
using Service.Services;
using Service.Utilities;

namespace Service.Notifications
{
    public class OperationsQueueNotification : IClientNotification
    {
        private readonly List<OperationModel> _operations;

        public OperationsQueueNotification(List<OperationModel> operations)
        {
            _operations = operations;
        }
        
        public void NotifySingle(Guid clientId = new Guid())
        {
            if (clientId == Guid.Empty) return;
            
            var concreteClient =
                ClientsRepository.RegisteredClients.SingleOrDefault(client => client.Id == clientId);

            concreteClient?.CallbacksApiChannel.UpdateOperationsQueue(_operations);
            
        }

        public void NotifyAll()
        {
            ClientsRepository.RegisteredClients.ExecuteCallbackMethod(client =>
                client.CallbacksApiChannel.UpdateOperationsQueue(_operations));
        }
    }
}