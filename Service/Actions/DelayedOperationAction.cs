using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts.Enums;
using Contracts.Models;
using Service.Services;


namespace Service.Actions
{
    public class DelayedOperationAction : IServiceAction
    {
        private readonly IClientsRepository _clientsRepository;

        private readonly DelayedOperations _delayedOperations;

        public DelayedOperationAction(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
            _delayedOperations = new DelayedOperations();
        }

        private void _sendInitialOperationsListToClients()
        {
            _clientsRepository.RegisteredClients.ForEach(client =>
                client.CallbackChannel.UpdateOperationsQueue(
                    _delayedOperations.OperationsList.FindAll(op => op.Status == OperationStatus.Idle)));
        }

        private void _sendIncompleteOperationsListToClients()
        {
            _clientsRepository.RegisteredClients.ForEach(client =>
                client.CallbackChannel.UpdateOperationsQueue(
                    _delayedOperations.OperationsList.FindAll(op => op.Status != OperationStatus.Completed)));
        }

        private void _sendCurrentlyProcessedOperationToClients(DelayedOperationModel currentDelayedOperation)
        {
            _clientsRepository.RegisteredClients.ForEach(client =>
                client.CallbackChannel.UpdateCurrentOperation(currentDelayedOperation));
        }

        private void _sendFinalMessageToClients(string message)
        {
            _clientsRepository.RegisteredClients.ForEach(clients =>
                clients.CallbackChannel.UpdateGeneralStatus(message));
        }

        public void Execute()
        {
            _sendInitialOperationsListToClients();
            
            foreach (var operation in _delayedOperations.OperationsList)
            {
                _sendCurrentlyProcessedOperationToClients(operation);
                
                Thread.Sleep(operation.Delay);
                
                operation.Status = OperationStatus.Completed;
                
                _sendIncompleteOperationsListToClients();
            }

            _sendFinalMessageToClients("All operations completed.");
        }
    }
}