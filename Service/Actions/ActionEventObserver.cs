using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Models;
using Service.Services;
using Service.Utilities;

namespace Service.Actions
{
    public static class ActionEventObserver
    {
        public static void OnRegistrationSuccess(Guid clientId)
        {
            var registeredClient = ClientsRepository.RegisteredClients.SingleOrDefault(client => client.Id == clientId);

            registeredClient?.CallbacksApiChannel.UpdateGeneralStatus("Client registered successfully!");
        }

        public static void OnOperationChange(OperationModel operationModel)
        {
            ClientsRepository.RegisteredClients.ExecuteCallbackMethod(client => client.CallbacksApiChannel.UpdateCurrentOperation(operationModel));
        }

        public static void OnOperationsListChange(List<OperationModel> operationModels)
        {
            ClientsRepository.RegisteredClients.ExecuteCallbackMethod(client =>
                client.CallbacksApiChannel.UpdateOperationsQueue(operationModels));
        }

        public static void OnOperationsCompleted()
        {
            ClientsRepository.RegisteredClients.ExecuteCallbackMethod(client => client.CallbacksApiChannel.UpdateGeneralStatus("All operations completed successfully."));
        }
    }
}