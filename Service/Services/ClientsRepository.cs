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

        private readonly Timer _maintenanceTimer;
        
        public ClientsRepository()
        {
            _maintenanceTimer = new Timer(10000);
            _maintenanceTimer.Elapsed += _maintenanceTimerOnElapsed;
            _maintenanceTimer.Enabled = true;
            _maintenanceTimer.AutoReset = true;
            _maintenanceTimer.Start();
        }

        public void StartMaintenance()
        {
            if(!_maintenanceTimer.Enabled)
                _maintenanceTimer.Start();
        }

        public void StopMaintenance()
        {
            if(_maintenanceTimer.Enabled)
                _maintenanceTimer.Stop();
        }

        private static void _maintenanceTimerOnElapsed(object sender, ElapsedEventArgs e)
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