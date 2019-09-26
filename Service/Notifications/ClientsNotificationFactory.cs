using System.Collections.Generic;
using Contracts.Models;
using Service.Clients;
using Service.Services;

namespace Service.Notifications
{
    public class ClientsNotificationFactory : IClientsNotificationFactory
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsNotificationFactory(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }
        
        public INotification GeneralStatus(string message)
        {
            return new GeneralStatusNotification(_clientsRepository, message);
        }

        public INotification CurrentOperation(SampleOperationModel currentOperation)
        {
            return new CurrentOperationNotification(_clientsRepository, currentOperation);
        }

        public INotification OperationsList(List<SampleOperationModel> operations)
        {
            return new OperationsListNotification(_clientsRepository, operations);
        }
    }
}