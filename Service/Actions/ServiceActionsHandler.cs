using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using Contracts;
using Contracts.Enums;
using Contracts.Models;
using Service.Clients;
using Service.Notifications;
using Service.Services;

namespace Service.Actions
{
    public class ServiceActionsHandler : IServiceActionsHandler
    {
        private static readonly object ThreadSyncObject = new object();

        private readonly List<IServiceAction> _serviceActionsQueue = new List<IServiceAction>();
        
        private readonly IClientsManagement _clientsManagement;

        private readonly INotificationFactory _notificationFactory;

        public ServiceActionsHandler(IClientsManagement clientsManagement, INotificationFactory notificationFactory)
        {
            _clientsManagement = clientsManagement;
            _notificationFactory = notificationFactory;
            
            var serviceActionsQueueProcessingTimer = new Timer(1000);
            serviceActionsQueueProcessingTimer.Elapsed += _executeActionFromQueueOnTimerElapsed;
            serviceActionsQueueProcessingTimer.Enabled = true;
            serviceActionsQueueProcessingTimer.AutoReset = true;
            serviceActionsQueueProcessingTimer.Start();
        }
        
        private void _executeActionFromQueueOnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (ThreadSyncObject)
            {
                try
                {
                    var action = _serviceActionsQueue.First();
                    
                    if(action == null)
                        return;
                    
                    action.Execute();

                    _serviceActionsQueue.Remove(action);
                }
                catch(InvalidOperationException){}
            }
        }
        
        public void PutActionInQueue(IServiceAction serviceAction)
        {
            lock (ThreadSyncObject)
            {
                _serviceActionsQueue.Add(serviceAction);
            }
        }

        public IServiceAction CreateActionFromModel(ActionModel actionModel)
        {
            switch (actionModel.Type)
            {
                case ActionType.SampleOperation:
                {
                    if (_clientsManagement.IsRegistered(actionModel.ClientId))
                        return new SampleOperationAction(_clientsManagement, _notificationFactory);
                    
                    return new InvalidAction();
                }

                case ActionType.RegisterClient:
                {
                    return new RegisterClientAction(actionModel.ClientId, _clientsManagement, _notificationFactory);
                }

                case ActionType.UnregisterClient:
                {
                    if (_clientsManagement.IsRegistered(actionModel.ClientId))
                        return new UnregisterClientAction(actionModel.ClientId, _clientsManagement);
                    
                    return new InvalidAction();
                }

                case ActionType.UpdateChannel:
                {
                    if (_clientsManagement.IsRegistered(actionModel.ClientId))
                        return new UpdateChannelAction(actionModel.ClientId, _clientsManagement);
                    
                    return new InvalidAction();
                }

                case ActionType.Invalid:
                {
                    return new InvalidAction();
                }

                default:
                    return new InvalidAction();
            }
            
        }
    }
}