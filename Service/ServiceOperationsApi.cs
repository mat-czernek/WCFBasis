using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Contracts;
using Contracts.Models;
using Service.Actions;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Service
{
    //TODO: extract some values as external configuration, like max time in seconds when client may be considered as inactive
    
    
    /// <summary>
    /// Class implements operations performed by WCF service
    /// All methods may be called by WCF client
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceOperationsApi : IServiceOperationsApi
    {
        /// <summary>
        /// Thread synchronization object
        /// </summary>
        private static readonly object ActionsSyncObject = new object();
        
        private static readonly object ClientsSyncObject = new object();
        
        /// <summary>
        /// The list of registered clients
        /// </summary>
        public static List<ClientModel> RegisteredClients = new List<ClientModel>();

        /// <summary>
        /// The list of service actions in queue
        /// </summary>
        private static readonly List<IServiceAction> ServiceActionsQueue = new List<IServiceAction>();

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static ServiceOperationsApi _instance;

        /// <summary>
        /// Creates the singleton instance of the class
        /// </summary>
        public static ServiceOperationsApi Instance => _instance ?? (_instance = new ServiceOperationsApi());

        /// <summary>
        /// Default constructor
        /// </summary>
        private ServiceOperationsApi()
        {
            var serviceActionsQueueProcessingTimer = new Timer(1000);
            serviceActionsQueueProcessingTimer.Elapsed += _serviceActionsQueueProcessingTimerOnElapsed;
            serviceActionsQueueProcessingTimer.Enabled = true;
            serviceActionsQueueProcessingTimer.AutoReset = true;
            serviceActionsQueueProcessingTimer.Start();

            var clientsMaintenanceTimer = new Timer(10000);
            clientsMaintenanceTimer.Elapsed += _clientsMaintenanceTimerOnElapsed;
            clientsMaintenanceTimer.Enabled = true;
            clientsMaintenanceTimer.AutoReset = true;
            clientsMaintenanceTimer.Start();
        }

        private static void _clientsMaintenanceTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (ClientsSyncObject)
            {
                // find inactive clients
                var inactiveClients = RegisteredClients
                    .FindAll(client => (DateTime.Now - client.LastActivityTime).Seconds >= 15);

                // remove inactive clients
                RegisteredClients = RegisteredClients.Except(inactiveClients).ToList();
            }
        }

        /// <summary>
        /// Timer method, process one action per cycle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _serviceActionsQueueProcessingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (ActionsSyncObject)
            {
                try
                {
                    var action = ServiceActionsQueue.First();
                    
                    if(action == null)
                        return;
                    
                    action.Take();

                    ServiceActionsQueue.Remove(action);
                }
                catch(InvalidOperationException){}
            }
        }
        
        /// <summary>
        /// Method produce action based on the client requirements
        /// </summary>
        /// <param name="actionModel">Action model</param>
        public void ActionRequest(ActionModel actionModel)
        {
            var actionsFactory = new ServiceActionsFactory();

            var action = actionsFactory.Create(actionModel);
            
            if(action is InvalidAction) return;
            
            if(actionModel.ExecuteImmediately)
            {
                action.Take();
            }
            else
            {
                lock (ActionsSyncObject)
                {
                    ServiceActionsQueue.Add(action);
                }
            }
        }
    }
}