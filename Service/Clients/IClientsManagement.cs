using System;
using System.Collections.Generic;
using Contracts.Models;
using Service.Notifications;

namespace Service.Clients
{
    public interface IClientsManagement
    {
        bool IsRegistered(Guid clientId);
        
        void Insert(IClientModel model);

        void Delete(Guid id);

        void Update(IClientModel model);
    }
}