using System;
using System.Linq;
using Contracts.Models;
using Service.Clients;

namespace Service.Notifications
{
    public class CurrentOperationNotification : INotification
    {
        private readonly IClientsRepository _clientsRepository;

        private readonly SampleOperationModel _currentOperation;
        
        public CurrentOperationNotification(IClientsRepository clientsRepository, SampleOperationModel currentOperation)
        {
            _clientsRepository = clientsRepository;
            _currentOperation = currentOperation;
        }
        
        public void NotifyById(Guid id)
        {
            var concreteClient = _clientsRepository.Clients.SingleOrDefault(client => client.Id == id);
            
            concreteClient?.CallbackChannel.UpdateCurrentOperation(_currentOperation);
        }

        public void NotifyAll()
        {
            _clientsRepository.Clients.ForEach(clients => clients.CallbackChannel.UpdateCurrentOperation(_currentOperation));
        }
    }
}