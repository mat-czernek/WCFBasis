using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;


namespace Service.Actions
{

    public class RegisterClientAction : IServiceAction
    {
        private readonly Guid _clientId;

        private readonly IClientsRepository _clientsRepository;

        public RegisterClientAction(Guid clientId, IClientsRepository clientsRepository)
        {
            _clientId = clientId;
            _clientsRepository = clientsRepository;
        }

        public void Execute()
        {
            if (_clientId == Guid.Empty) return;
            
            if (_clientsRepository.IsRegisteredClient(_clientId)) return;

            var newClient = new ClientModel() { Id = _clientId, 
                CallbackChannel = OperationContext.Current.GetCallbackChannel<IClientCallbackContract>() };
            
            _clientsRepository.Insert(newClient);
            
            newClient.CallbackChannel.UpdateGeneralStatus("Client registered successfully.");
        }
    }
}