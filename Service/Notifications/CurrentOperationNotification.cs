using System;
using System.Linq;
using System.Runtime.InteropServices;
using Contracts.Models;
using Service.Services;
using Service.Utilities;

namespace Service.Notifications
{
    public class CurrentOperationNotification : IClientNotification
    {
        private readonly OperationModel _currentOperation;

        public CurrentOperationNotification(OperationModel currentOperation)
        {
            _currentOperation = currentOperation;
        }
        
        public void NotifySingle(Guid clientId = new Guid())
        {
            if (clientId == Guid.Empty) return;
            
            var concreteClient =
                ClientsRepository.RegisteredClients.SingleOrDefault(client => client.Id == clientId);

            concreteClient?.CallbacksApiChannel.UpdateCurrentOperation(_currentOperation);
            
        }

        public void NotifyAll()
        {
            ClientsRepository.RegisteredClients.ExecuteCallbackMethod(client =>
                client.CallbacksApiChannel.UpdateCurrentOperation(_currentOperation));
        }
    }
}