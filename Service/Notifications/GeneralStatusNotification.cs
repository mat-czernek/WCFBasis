using System;
using System.Linq;
using Service.Clients;
using Service.Services;


namespace Service.Notifications
{
    public class GeneralStatusNotification : INotification
    {
        private readonly IClientsRepository _clientsRepository;

        private readonly string _message;

        public GeneralStatusNotification(IClientsRepository clientsRepository, string message)
        {
            _message = message;
            _clientsRepository = clientsRepository;
        }
        
        public void NotifyById(Guid id)
        {
            var concreteClient = _clientsRepository.Clients.SingleOrDefault(client => client.Id == id);
            
            concreteClient?.CallbackChannel.UpdateGeneralStatus(_message);
        }

        public void NotifyAll()
        {
            _clientsRepository.Clients.ForEach(clients => clients.CallbackChannel.UpdateGeneralStatus(_message));
        }
    }
}