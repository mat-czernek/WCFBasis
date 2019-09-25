using System;
using System.Collections.Generic;
using Contracts.Models;

namespace Service.Services
{
    public interface IClientsRepository
    {
        bool IsRegisteredClient(Guid clientId);
        
        List<IClientModel> RegisteredClients { get; }

        void Insert(IClientModel model);

        void Delete(Guid id);

        void Update(IClientModel model);
    }
}