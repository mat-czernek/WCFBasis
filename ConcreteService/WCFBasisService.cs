﻿using System.ServiceProcess;
using Service;
using Service.Actions;
using Service.Clients;
using Service.Notifications;
using Service.Services;

namespace ConcreteService
{
    public partial class WCFBasisService : ServiceBase
    {
        private readonly WcfServiceHost _wcfServiceHost;
        
        public WCFBasisService()
        {
            InitializeComponent();
            
            var clientsRepository = new ClientsRepository();
            var clientsNotifications = new NotificationFactory(clientsRepository);
            var clientsManagement = new ClientsManagement(clientsRepository);
            var actionsHandler = new ServiceActionsHandler(clientsManagement, clientsNotifications);
            
            _wcfServiceHost = new WcfServiceHost(new ServiceContract(actionsHandler));
        }

        protected override void OnStart(string[] args)
        {
            _wcfServiceHost.Open();
        }

        protected override void OnStop()
        {
            _wcfServiceHost.Close();
        }
    }
}
