using System.Collections.Generic;
using Contracts.Models;

namespace Service.Notifications
{
    public interface IClientsNotificationFactory
    {
        INotification GeneralStatus(string message);

        INotification CurrentOperation(SampleOperationModel currentOperation);

        INotification OperationsList(List<SampleOperationModel> operations);
    }
}