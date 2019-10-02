using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Clients;
using Service.Services;

namespace Service.Actions
{
    public class UpdateChannelAction : IServiceAction
    {
        private static readonly object SyncObject = new object();

        private readonly Guid _clientId;

        private readonly IClientsManagement _clientsManagement;
        
        public UpdateChannelAction(Guid clientId, IClientsManagement clientsManagement)
        {
            _clientId = clientId;
            _clientsManagement = clientsManagement;
        }
        
        public void Execute()
        {
            var clientToUpdate = new ClientModel()
            {
                Id = _clientId,
                CallbackChannel = OperationContext.Current.GetCallbackChannel<ICallbackContract>(),
                LastActivityTime = DateTime.Now
            };
            
            _clientsManagement.Update(clientToUpdate);
        }
    }
}