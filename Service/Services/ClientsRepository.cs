using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Contracts.Models;

namespace Service.Services
{
    public class ClientsRepository : IClientsRepository
    {
        private static readonly object ThreadSyncObject = new object();

        public List<IClientModel> RegisteredClients { get; private set; }

        public bool IsRegisteredClient(Guid clientId)
        {
            lock (ThreadSyncObject)
            {
                var concreteClient = RegisteredClients.SingleOrDefault(client => client.Id == clientId);
                
                return concreteClient != null;
            }
            
        }
        
        public ClientsRepository()
        {
            RegisteredClients = new List<IClientModel>();
            
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
                RegisteredClients.Add(model);
            }
        }


        public void Delete(Guid id)
        {
            if(id == Guid.Empty) return;
            
            lock (ThreadSyncObject)
            {
                var clientToDelete = RegisteredClients.SingleOrDefault(client => client.Id == id);
                
                if (clientToDelete == null) return;
                
                RegisteredClients.Remove(clientToDelete);
            }
        }


        public void Update(IClientModel model)
        {
            if(model == null) return;
            
            if(model.Id == Guid.Empty) return;
            
            lock (ThreadSyncObject)
            {
                var clientToUpdate = RegisteredClients.SingleOrDefault(client => client.Id == model.Id);
                
                if(clientToUpdate == null) return;

                clientToUpdate.CallbackChannel = model.CallbackChannel;
                clientToUpdate.LastActivityTime = model.LastActivityTime;
            }
        }


        private void _removeInactiveClients(object sender, ElapsedEventArgs e)
        {
            lock (ThreadSyncObject)
            {
                var inactiveClients = RegisteredClients
                    .FindAll(client => (DateTime.Now - client.LastActivityTime).Seconds >= 15);
                
                RegisteredClients = RegisteredClients.Except(inactiveClients).ToList();
            }
        }
    }
}