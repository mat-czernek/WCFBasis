using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;

namespace Service.Actions
{
    public class UpdateChannelAction : IServiceAction
    {
        private static readonly object SyncObject = new object();

        private readonly Guid _clientId;

        private readonly IClientsRepository _clientsRepository;
        
        public UpdateChannelAction(Guid clientId, IClientsRepository clientsRepository)
        {
            _clientId = clientId;
            _clientsRepository = clientsRepository;
        }
        
        public void Execute()
        {
            var clientToUpdate = new ClientModel()
            {
                Id = _clientId,
                CallbackChannel = OperationContext.Current.GetCallbackChannel<IClientCallbackContract>(),
                LastActivityTime = DateTime.Now
            };
            
            _clientsRepository.Update(clientToUpdate);
        }
    }
}