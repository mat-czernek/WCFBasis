using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Enums;
using Contracts.Models;
using Service.Clients;
using Service.Services;

namespace Service.Actions
{
 
    public class UnregisterClientAction : IServiceAction
    {
        private static readonly object SyncObject = new object();

        private readonly Guid _clientId;
        
        private readonly IClientsManagement _clientsManagement;
        
        public UnregisterClientAction(Guid clientId, IClientsManagement clientsManagement)
        {
            _clientId = clientId;
            _clientsManagement = clientsManagement;
        }
        
        public void Execute()
        {
            if (_clientId == Guid.Empty) return;

            _clientsManagement.Delete(_clientId);
        }
    }
}