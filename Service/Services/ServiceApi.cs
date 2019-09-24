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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceApi : IServiceApi
    {
        private static readonly object ThreadSyncObject = new object();
        
        private static readonly List<IServiceAction> ServiceActionsQueue = new List<IServiceAction>();

        private readonly IServiceActionsFactory _serviceActionsFactory;
        
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
        
        
        private static void _serviceActionsQueueProcessingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (ThreadSyncObject)
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
        /// Method called by client.
        /// </summary>
        /// <param name="actionModel">Requested action model describing what type of actions should be taken by service</param>
        public void ActionRequest(ActionModel actionModel)
        {
            var requestedAction = _serviceActionsFactory.Create(actionModel);
            
            if(requestedAction is InvalidAction) return;
            
            if(actionModel.ExecuteImmediately)
            {
                requestedAction.Take();
            }
            else
            {
                lock (ThreadSyncObject)
                {
                    ServiceActionsQueue.Add(requestedAction);
                }
            }
        }
    }
}