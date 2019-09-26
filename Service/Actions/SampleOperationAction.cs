using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts.Enums;
using Contracts.Models;
using Service.Clients;
using Service.Services;


namespace Service.Actions
{
    public class SampleOperationAction : IServiceAction
    {
        private readonly IClientsManagement _clientsManagement;

        private readonly SampleOperations _sampleOperations;

        public SampleOperationAction(IClientsManagement clientsManagement)
        {
            _clientsManagement = clientsManagement;
            _sampleOperations = new SampleOperations();
        }
        
        public void Execute()
        {
            _clientsManagement.NotificationFactory.OperationsList(
                _sampleOperations.OperationsList.FindAll(op => op.Status == OperationStatus.Idle)
                ).NotifyAll();
            
            foreach (var operation in _sampleOperations.OperationsList)
            {
                _clientsManagement.NotificationFactory.CurrentOperation(operation).NotifyAll();
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;
                
                _clientsManagement.NotificationFactory.OperationsList(
                    _sampleOperations.OperationsList.FindAll(op => op.Status != OperationStatus.Completed)
                ).NotifyAll();
            }
            
            _clientsManagement.NotificationFactory.GeneralStatus("All operations completed.").NotifyAll();
        }
    }
}