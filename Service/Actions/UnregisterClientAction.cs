using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;

namespace Service.Actions
{
 
    public class UnregisterClientAction : IServiceAction
    {
        private static readonly object SyncObject = new object();

        private readonly Guid _clientId;
        
        private readonly IClientsRepository _clientsRepository;
        
        public UnregisterClientAction(Guid clientId, IClientsRepository clientsRepository)
        {
            _clientId = clientId;
            _clientsRepository = clientsRepository;
        }
        
        public void Execute()
        {
            if (_clientId == Guid.Empty) return;

            _clientsRepository.Delete(_clientId);
        }
    }
}