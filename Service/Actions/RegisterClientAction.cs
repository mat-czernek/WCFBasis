using System;
using System.Collections.Generic;
using System.ServiceModel;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Clients;
using Service.Notifications;
using Service.Services;


namespace Service.Actions
{
    public class RegisterClientAction : IServiceAction
    {
        private readonly Guid _clientId;

        private readonly IClientsManagement _clientsManagement;
        
        public RegisterClientAction(Guid clientId, IClientsManagement clientsManagement)
        {
            _clientId = clientId;
            _clientsManagement = clientsManagement;
        }

        public void Execute()
        {
            if (_clientId == Guid.Empty) return;
            
            if (_clientsManagement.IsRegistered(_clientId)) return;

            var newClient = new ClientModel() { Id = _clientId, 
                CallbackChannel = OperationContext.Current.GetCallbackChannel<IClientCallbackContract>() };
            
            _clientsManagement.Insert(newClient);
            
            _clientsManagement.NotificationFactory.GeneralStatus("Registered successfully.").NotifyById(_clientId);
        }
    }
}