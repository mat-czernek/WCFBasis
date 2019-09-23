using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Contracts.Models;

namespace Service.Services
{
    public class ClientsRepository : IClientsRepository
    {
        private static readonly object SyncObject = new object();

        public static List<ClientModel> RegisteredClients = new List<ClientModel>();

        private readonly Timer _monitoringTimer;
        
        public ClientsRepository()
        {
            _monitoringTimer = new Timer(10000);
            _monitoringTimer.Elapsed += _clientsMonitoring;
            _monitoringTimer.Enabled = true;
            _monitoringTimer.AutoReset = true;
            _monitoringTimer.Start();
        }

        public void StartMonitoring()
        {
            if(!_monitoringTimer.Enabled)
                _monitoringTimer.Start();
        }

        public void StopMonitoring()
        {
            if(_monitoringTimer.Enabled)
                _monitoringTimer.Stop();
        }

        private static void _clientsMonitoring(object sender, ElapsedEventArgs e)
        {
            lock (SyncObject)
            {
                // find inactive clients
                var inactiveClients = RegisteredClients
                    .FindAll(client => (DateTime.Now - client.LastActivityTime).Seconds >= 15);
                
                // remove inactive clients
                RegisteredClients = RegisteredClients.Except(inactiveClients).ToList();
            }
        }
    }
}