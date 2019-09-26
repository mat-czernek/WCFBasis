using System.Collections.Generic;
using Contracts.Models;

namespace Service.Clients
{
    public interface IClientsRepository
    {
        List<IClientModel> Clients { get; set; }
    }
}