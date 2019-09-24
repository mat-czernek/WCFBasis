using System;

namespace Service.Notifications
{
    public interface IClientNotification
    {
        void NotifySingle(Guid clientId);

        void NotifyAll();
    }
}