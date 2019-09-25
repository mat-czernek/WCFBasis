using System;
using System.Linq;
using Service.Services;


namespace Service.Notifications
{
    public class GeneralNotification : INotification
    {
        private readonly IClientsRepository _clientsRepository;

        private readonly string _message;

        public GeneralNotification(IClientsRepository clientsRepository, string message)
        {
            _message = message;
            _clientsRepository = clientsRepository;
        }
        
        public void NotifyById(Guid id)
        {
            var concreteClient = _clientsRepository.RegisteredClients.SingleOrDefault(client => client.Id == id);
            
            concreteClient?.CallbackChannel.UpdateGeneralStatus(_message);
        }

        public void NotifyAll()
        {
            _clientsRepository.RegisteredClients.ForEach(clients => clients.CallbackChannel.UpdateGeneralStatus(_message));
        }
    }
}