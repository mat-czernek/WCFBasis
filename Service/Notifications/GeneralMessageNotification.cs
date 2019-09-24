using System;
using System.Linq;
using Service.Services;
using Service.Utilities;

namespace Service.Notifications
{
    public class GeneralMessageNotification : IClientNotification
    {
        private readonly string _message;

        public GeneralMessageNotification(string message)
        {
            _message = message;
        }
        
        public void NotifySingle(Guid clientId)
        {
            if(clientId == Guid.Empty) return;
            
            var concreteClient =
                ClientsRepository.RegisteredClients.SingleOrDefault(client => client.Id == clientId);

            concreteClient?.CallbacksApiChannel.UpdateGeneralStatus(_message);
        }

        public void NotifyAll()
        {
            ClientsRepository.RegisteredClients.ExecuteCallbackMethod(client =>
                client.CallbacksApiChannel.UpdateGeneralStatus(_message));
        }
    }
}