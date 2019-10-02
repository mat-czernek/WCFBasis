using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts.Enums;
using Contracts.Models;
using Service.Clients;
using Service.Notifications;
using Service.Services;


namespace Service.Actions
{
    public class SampleOperationAction : IServiceAction
    {
        private readonly IClientsManagement _clientsManagement;

        private readonly SampleOperations _sampleOperations;

        private readonly INotificationFactory _notificationFactory;

        public SampleOperationAction(IClientsManagement clientsManagement, INotificationFactory notificationFactory)
        {
            _clientsManagement = clientsManagement;
            _sampleOperations = new SampleOperations();
            _notificationFactory = notificationFactory;
        }
        
        public void Execute()
        {
            _notificationFactory.OperationsList(
                _sampleOperations.OperationsList.FindAll(op => op.Status == OperationStatus.Idle)
                ).NotifyAll();
            
            foreach (var operation in _sampleOperations.OperationsList)
            {
                _notificationFactory.CurrentOperation(operation).NotifyAll();
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;
                
                _notificationFactory.OperationsList(
                    _sampleOperations.OperationsList.FindAll(op => op.Status != OperationStatus.Completed)
                ).NotifyAll();
            }
            
            _notificationFactory.GeneralStatus("All operations completed.").NotifyAll();
        }
    }
}