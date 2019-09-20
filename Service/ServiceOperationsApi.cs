using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Contracts.Delegates;
using Contracts.Enums;
using Contracts.Models;
using Service.Actions;
using System.Timers;
using Service.Services;
using Timer = System.Timers.Timer;

namespace Service
{
    /// <summary>
    /// Class implements operations performed by WCF service
    /// All methods may be called by WCF client
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true, AutomaticSessionShutdown = false)]
    public class ServiceOperationsApi : IServiceOperationsApi
    {
        /// <summary>
        /// Thread synchronization objecy
        /// </summary>
        private static readonly object SyncObject = new object();
        
        /// <summary>
        /// The list of registered clients
        /// </summary>
        public static readonly List<ClientModel> RegisteredClients = new List<ClientModel>();

        private static readonly ObservableCollection<IAction> ActionsQueue = new ObservableCollection<IAction>();
        
        private readonly Timer _processQueue;
        
        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static ServiceOperationsApi _instance = null;

        /// <summary>
        /// Creates the singleton instance of the class
        /// </summary>
        public static ServiceOperationsApi Instance
        {
            get
            {
                lock (SyncObject)
                {
                    return _instance ?? (_instance = new ServiceOperationsApi());
                }
            }
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        private ServiceOperationsApi()
        {
            _processQueue = new Timer(1000);
            _processQueue.Elapsed += _processQueueOnElapsed;
            _processQueue.Enabled = true;
            _processQueue.AutoReset = true;
            _processQueue.Start();
        }

        private void _processQueueOnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (SyncObject)
            {
                try
                {
                    var action = ActionsQueue.First();
                    
                    if(action == null)
                        return;
                    
                    action.Take();

                    ActionsQueue.Remove(action);
                }
                catch(InvalidOperationException){}
            }
        }


        public void ActionRequest(ActionType actionType, Guid clientId)
        {
            if (actionType == ActionType.UpdateChannel)
            {
                new UpdateChannelAction(clientId, OperationContext.Current.GetCallbackChannel<ICallbacksApi>()).Take();
            }
            
            if (actionType == ActionType.UnregisterClient)
            {
                new UnregisterClientAction(clientId).Take();
            }
            
            if (actionType == ActionType.RegisterClient)
            {
                new RegisterClientAction(clientId, OperationContext.Current.GetCallbackChannel<ICallbacksApi>()).Take();
            }
            
            
            switch (actionType)
            {
                case ActionType.SampleOperation:
                    if(ActionsQueue.All(action => action.Type != ActionType.SampleOperation))
                        ActionsQueue.Add(new SampleOperationAction(clientId));
                    break;
                
                default:
                    break;
            }
        }
    }
}