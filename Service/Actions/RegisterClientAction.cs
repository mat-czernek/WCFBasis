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

        private readonly INotificationFactory _notificationFactory;
        
        public RegisterClientAction(Guid clientId, IClientsManagement clientsManagement, INotificationFactory notificationFactory)
        {
            _clientId = clientId;
            _clientsManagement = clientsManagement;
            _notificationFactory = notificationFactory;
        }

        public void Execute()
        {
            if (_clientId == Guid.Empty) return;
            
            if (_clientsManagement.IsRegistered(_clientId)) return;

            var newClient = new ClientModel() { Id = _clientId, 
                CallbackChannel = OperationContext.Current.GetCallbackChannel<ICallbackContract>() };
            
            _clientsManagement.Insert(newClient);
            
            _notificationFactory.GeneralStatus("Registered successfully.").NotifyById(_clientId);
        }
    }
}