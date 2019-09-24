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

        public static List<ClientModel> RegisteredClients = new List<ClientModel>();

        private readonly Timer _monitorInactiveClients;

        public static bool IsRegisteredClient(Guid clientId)
        {
            lock (ThreadSyncObject)
            {
                var concreteClient = RegisteredClients.SingleOrDefault(client => client.Id == clientId);
                
                return concreteClient != null;
            }
            
        }
        
        public ClientsRepository()
        {
            _monitorInactiveClients = new Timer(10000);
            _monitorInactiveClients.Elapsed += _removeInactiveClients;
            _monitorInactiveClients.Enabled = true;
            _monitorInactiveClients.AutoReset = true;
            _monitorInactiveClients.Start();
        }

        public void StartMonitoring()
        {
            if(!_monitorInactiveClients.Enabled)
                _monitorInactiveClients.Start();
        }

        public void StopMonitoring()
        {
            if(_monitorInactiveClients.Enabled)
                _monitorInactiveClients.Stop();
        }

        private static void _removeInactiveClients(object sender, ElapsedEventArgs e)
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