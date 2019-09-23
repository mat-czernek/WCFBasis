using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using Contracts;
using Contracts.Models;
using Service.Actions;
using Timer = System.Timers.Timer;

namespace Service.Services
{
    //TODO: extract some values as external configuration, like max time in seconds when client may be considered as inactive
    //TODO: cleanup and re-factor the client side, including the callback methods
    //TODO: check the service behavior when stopping the host, what should be cleaned up, close, etc. when Windows Service stop
    
    /// <summary>
    /// Class implements operations performed by WCF service
    /// All methods may be called by WCF client
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceApi : IServiceApi
    {
        /// <summary>
        /// Thread synchronization object
        /// </summary>
        private static readonly object ActionsSyncObject = new object();

        /// <summary>
        /// The list of service actions in queue
        /// </summary>
        private static readonly List<IServiceAction> ServiceActionsQueue = new List<IServiceAction>();

        private readonly IServiceActionsFactory _serviceActionsFactory;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceApi(IClientsRepository clientsRepository, 
            IServiceActionsFactory serviceActionsFactory)
        {
            clientsRepository.StartMonitoring();
            
            _serviceActionsFactory = serviceActionsFactory;

            var serviceActionsQueueProcessingTimer = new Timer(1000);
            serviceActionsQueueProcessingTimer.Elapsed += _serviceActionsQueueProcessingTimerOnElapsed;
            serviceActionsQueueProcessingTimer.Enabled = true;
            serviceActionsQueueProcessingTimer.AutoReset = true;
            serviceActionsQueueProcessingTimer.Start();
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
            var action = _serviceActionsFactory.Create(actionModel);
            
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