using System.Collections.Generic;
using Contracts.Models;

namespace Service.Clients
{
    public class ClientsRepository : IClientsRepository
    {
        public List<IClientModel> Clients { get; set; }

        public ClientsRepository()
        {
            Clients = new List<IClientModel>();
        }
    }
}