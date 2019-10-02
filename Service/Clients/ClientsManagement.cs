using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Contracts.Models;
using Service.Notifications;

namespace Service.Clients
{
    public class ClientsManagement : IClientsManagement
    {
        private static readonly object ThreadSyncObject = new object();
        
        private readonly IClientsRepository _clientsRepository;
        
        public bool IsRegistered(Guid clientId)
        {
            lock (ThreadSyncObject)
            {
                var concreteClient = _clientsRepository.Clients.SingleOrDefault(client => client.Id == clientId);
                
                return concreteClient != null;
            }
            
        }
        
        public ClientsManagement(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
            
            var monitorInactiveClients = new Timer(10000);
            monitorInactiveClients.Elapsed += _removeInactiveClients;
            monitorInactiveClients.Enabled = true;
            monitorInactiveClients.AutoReset = true;
            monitorInactiveClients.Start();
        }

        
        public void Insert(IClientModel model)
        {
            if(model == null) return;
            
            if(model.Id == Guid.Empty) return;
            
            model.RegistrationTime = DateTime.Now;
            model.LastActivityTime = DateTime.Now;

            lock (ThreadSyncObject)
            {
                _clientsRepository.Clients.Add(model);
            }
        }


        public void Delete(Guid id)
        {
            if(id == Guid.Empty) return;
            
            lock (ThreadSyncObject)
            {
                var clientToDelete = _clientsRepository.Clients.SingleOrDefault(client => client.Id == id);
                
                if (clientToDelete == null) return;
                
                _clientsRepository.Clients.Remove(clientToDelete);
            }
        }


        public void Update(IClientModel model)
        {
            if(model == null) return;
            
            if(model.Id == Guid.Empty) return;
            
            lock (ThreadSyncObject)
            {
                var clientToUpdate = _clientsRepository.Clients.SingleOrDefault(client => client.Id == model.Id);
                
                if(clientToUpdate == null) return;

                clientToUpdate.CallbackChannel = model.CallbackChannel;
                clientToUpdate.LastActivityTime = model.LastActivityTime;
            }
        }


        private void _removeInactiveClients(object sender, ElapsedEventArgs e)
        {
            lock (ThreadSyncObject)
            {
                var inactiveClients = _clientsRepository.Clients
                    .FindAll(client => (DateTime.Now - client.LastActivityTime).Seconds >= 15);

                foreach (var inactiveClient in inactiveClients)
                {
                    Console.WriteLine($"Inactive client: {inactiveClient.Id}");
                }
                
                _clientsRepository.Clients = _clientsRepository.Clients.Except(inactiveClients).ToList();
            }
        }
    }
}