using System.Collections.Generic;
using Contracts.Models;

namespace Service.Notifications
{
    internal static class ClientNotificationFactory
    {
        public static IClientNotification GeneralNotification(string message)
        {
            return new GeneralMessageNotification(message);
        }

        public static IClientNotification OperationsQueueUpdate(List<OperationModel> operations)
        {
            return new OperationsQueueNotification(operations);
        }

        public static IClientNotification CurrentOperationUpdate(OperationModel operation)
        {
            return new CurrentOperationNotification(operation);
        }
    }
}