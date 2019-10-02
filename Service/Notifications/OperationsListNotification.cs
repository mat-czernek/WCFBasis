using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Models;
using Service.Clients;

namespace Service.Notifications
{
    public class OperationsListNotification : INotification
    {
        private readonly IClientsRepository _clientsRepository;

        private readonly List<SampleOperationModel> _operations;

        public OperationsListNotification(IClientsRepository clientsRepository, List<SampleOperationModel> operations)
        {
            _clientsRepository = clientsRepository;
            _operations = operations;
        }
        
        public void NotifyById(Guid id)
        {
            var concreteClient = _clientsRepository.Clients.SingleOrDefault(client => client.Id == id);
            
            concreteClient?.CallbackChannel.NotifyOperationQueueChanged(_operations);
        }

        public void NotifyAll()
        {
            _clientsRepository.Clients.ForEach(clients => clients.CallbackChannel.NotifyOperationQueueChanged(_operations));
        }
    }
}